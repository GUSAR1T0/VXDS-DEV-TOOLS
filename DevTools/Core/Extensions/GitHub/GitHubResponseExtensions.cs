using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VXDesign.Store.DevTools.Core.Entities.GitHub.Base;
using VXDesign.Store.DevTools.Core.Extensions.Base;
using VXDesign.Store.DevTools.Core.Extensions.HTTP;

namespace VXDesign.Store.DevTools.Core.Extensions.GitHub
{
    internal static class GitHubResponseExtensions
    {
        internal static TResponse PostHandle<TResponse>(this TResponse response) where TResponse : IGitHubResponse
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy(true, true)
                }
            };

            if (response.IsWithoutErrors())
            {
                var property = response.GetType().GetProperty(nameof(IntermediateGitHubResponse<object>.Response));
                var value = JsonConvert.DeserializeObject(response.Output, property.PropertyType, settings);
                property.SetPropertyValue(response, value);
            }

            return response;
        }
    }
}