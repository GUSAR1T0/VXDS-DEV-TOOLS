using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.HTTP;
using VXDesign.Store.DevTools.Common.Enums.Camunda;
using VXDesign.Store.DevTools.Common.Services.Syrinx;
using VXDesign.Store.DevTools.Common.Utils.Camunda;

namespace VXDesign.Store.DevTools.Common.Containers.Camunda.Base
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
                    ContractResolver = new RequestJsonResolver(),
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
                            switch (value)
                            {
                                case IEnumerable<object> enumerable:
                                    return new { name = property.Name, value = string.Join(",", enumerable) };
                                default:
                                    return new { name = property.Name, value = value.ToString() };
                            }
                        }).ToDictionary(property => property.name, property => property.value),
                    Body = JsonConvert.SerializeObject(this, settings)
                }, settings);
            }
        }

        public async Task<TResponse> SendRequest(ISyrinxCamundaClientService service) => await service.Send<CamundaRequest<TResponse>, TResponse>(this);
    }
}