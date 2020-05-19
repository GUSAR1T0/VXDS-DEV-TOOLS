using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Utils;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.HTTP;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Base
{
    public interface ICamundaRequest : IRequest
    {
        CamundaAction Action { get; }
    }

    public abstract class CamundaRequest<TResponse> : ICamundaRequest where TResponse : ICamundaResponse, new()
    {
        [JsonIgnore]
        public abstract CamundaAction Action { get; }

        [JsonIgnore]
        public Dictionary<string, string> Path => new Dictionary<string, string>();

        [JsonIgnore]
        public Dictionary<string, string> Query => new Dictionary<string, string>();

        [JsonIgnore]
        public string Body
        {
            get
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamundaRequestJsonResolver(Resources?.Any() != true),
                    NullValueHandling = NullValueHandling.Ignore
                };
                return JsonConvert.SerializeObject(new
                {
                    Action,
                    Path = JsonConvert.SerializeObject(GetType().GetFields().Select(field => new { name = field.Name, value = field.GetValue(this)?.ToString() })
                        .ToDictionary(field => field.name, field => field.value)),
                    Query = JsonConvert.SerializeObject(GetType().GetProperties()
                        .Where(property => property.GetCustomAttributes<HttpQueryParameterAttribute>().Any() && !string.IsNullOrWhiteSpace(property.GetValue(this)?.ToString()))
                        .Select(property =>
                        {
                            var value = property.GetValue(this);
                            return value switch
                            {
                                IEnumerable<object> enumerable => new { name = property.Name, value = string.Join(",", enumerable) },
                                bool boolean => new { name = property.Name, value = boolean.ToString().ToLower() },
                                _ => new { name = property.Name, value = value.ToString() }
                            };
                        }).ToDictionary(property => property.name, property => property.value)),
                    Body = JsonConvert.SerializeObject(this, settings)
                }, settings);
            }
        }

        [JsonIgnore]
        public IReadOnlyList<LocalFile> Resources => GetType().GetProperties()
            .Where(property => property.GetCustomAttributes<HttpFileParameterAttribute>().Any())
            .Select(property => property.GetValue(this) as IReadOnlyList<LocalFile>)
            .SelectMany(x => x).ToList();

        public async Task<TResponse> SendRequest(IOperation operation, ISyrinxCamundaClientService service, bool handleError = false)
        {
            var response = await service.Send<CamundaRequest<TResponse>, TResponse>(operation, this);

            if (handleError && !response.IsWithoutErrors())
            {
                throw CommonExceptions.CamundaRequestIsFailed(operation, response.Status, response.Reason, response.Output);
            }

            return response;
        }
    }
}