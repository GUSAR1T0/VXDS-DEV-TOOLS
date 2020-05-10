using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Utils;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.HTTP;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Base
{
    public interface IGitHubRequest : IRequest
    {
        GitHubAction Action { get; }
    }

    public abstract class GitHubRequest<TResponse> : IGitHubRequest where TResponse : IGitHubResponse, new()
    {
        [JsonIgnore]
        public abstract GitHubAction Action { get; }

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
            ContractResolver = new GitHubRequestJsonResolver(),
            NullValueHandling = NullValueHandling.Ignore
        });

        [JsonIgnore]
        public IReadOnlyList<LocalFile> Resources => new List<LocalFile>();

        public async Task<TResponse> SendRequest(IOperation operation, IGitHubClientService service) => await service.Send<GitHubRequest<TResponse>, TResponse>(operation, this);
    }
}