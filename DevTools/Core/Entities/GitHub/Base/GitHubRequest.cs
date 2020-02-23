using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Core.Attributes;
using VXDesign.Store.DevTools.Core.Entities.HTTP;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Services.GitHub;
using VXDesign.Store.DevTools.Core.Utils.GitHub;
using GitHubEndpointEnum = VXDesign.Store.DevTools.Core.Enums.GitHub.GitHubEndpoint;

namespace VXDesign.Store.DevTools.Core.Entities.GitHub.Base
{
    public interface IGitHubRequest : IRequest
    {
        GitHubEndpointEnum Endpoint { get; }
    }

    public abstract class GitHubRequest<TResponse> : IGitHubRequest where TResponse : IGitHubResponse, new()
    {
        [JsonIgnore]
        public abstract GitHubEndpointEnum Endpoint { get; }

        [JsonIgnore]
        public Dictionary<string, string> Path => GetType().GetFields().Select(field => new { name = field.Name, value = field.GetValue(this)?.ToString() })
            .ToDictionary(field => field.name, field => field.value);

        [JsonIgnore]
        public Dictionary<string, string> Query => GetType().GetProperties()
            .Where(property => property.GetCustomAttributes<HttpQueryParameterAttribute>().Any() && !string.IsNullOrWhiteSpace(property.GetValue(this)?.ToString()))
            .Select(property =>
            {
                var value = property.GetValue(this);
                return value switch
                {
                    IEnumerable<object> enumerable => new { name = property.Name, value = string.Join(",", enumerable) },
                    _ => new { name = property.Name, value = value.ToString() }
                };
            }).ToDictionary(property => property.name, property => property.value);

        [JsonIgnore]
        public string Body => JsonConvert.SerializeObject(this, new JsonSerializerSettings
        {
            ContractResolver = new RequestJsonResolver(),
            NullValueHandling = NullValueHandling.Ignore
        });

        public async Task<TResponse> SendRequest(IOperation operation, IGitHubClientService service) => await service.Send<GitHubRequest<TResponse>, TResponse>(operation, this);
    }
}