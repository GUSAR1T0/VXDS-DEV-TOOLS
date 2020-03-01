using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Utils;
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
                    ContractResolver = new CamundaRequestJsonResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                };
                return JsonConvert.SerializeObject(new
                {
                    Action,
                    Path = GetType().GetFields().Select(field => new { name = field.Name, value = field.GetValue(this)?.ToString() })
                        .ToDictionary(field => field.name, field => field.value),
                    Query = GetType().GetProperties()
                        .Where(property => property.GetCustomAttributes<HttpQueryParameterAttribute>().Any() && !string.IsNullOrWhiteSpace(property.GetValue(this)?.ToString()))
                        .Select(property =>
                        {
                            var value = property.GetValue(this);
                            return value switch
                            {
                                IEnumerable<object> enumerable => new { name = property.Name, value = string.Join(",", enumerable) },
                                _ => new { name = property.Name, value = value.ToString() }
                            };
                        }).ToDictionary(property => property.name, property => property.value),
                    Body = JsonConvert.SerializeObject(this, settings)
                }, settings);
            }
        }

        public async Task<TResponse> SendRequest(IOperation operation, ISyrinxCamundaClientService service) => await service.Send<CamundaRequest<TResponse>, TResponse>(operation, this);
    }
}