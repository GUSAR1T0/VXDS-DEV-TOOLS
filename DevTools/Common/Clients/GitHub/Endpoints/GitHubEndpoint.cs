using System;
using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Endpoints
{
    public class GitHubEndpoint
    {
        public int ActionCode { get; }
        public string ActionName { get; }
        public HttpMethod Method { get; }
        public string Path { get; }

        private GitHubEndpoint(GitHubAction gitHubAction, GitHubEndpointAttribute gitHubEndpointAttribute)
        {
            ActionCode = (int) gitHubAction;
            ActionName = gitHubEndpointAttribute.Description;
            Method = gitHubEndpointAttribute.Method;
            Path = '/' + gitHubEndpointAttribute.Path;
        }

        private static IEnumerable<GitHubEndpoint> GetEndpoints(Func<GitHubAction, bool> predicate = null)
        {
            return
                from endpoint in EnumExtensions.GetValues<GitHubAction>().Where(predicate ?? (action => true))
                let attribute = endpoint.GetAttributeOfType<GitHubEndpointAttribute>()
                select new GitHubEndpoint(endpoint, attribute);
        }

        public static IEnumerable<GitHubEndpoint> GetAll() => GetEndpoints().ToList();

        public static GitHubEndpoint GetEndpoint(GitHubAction gitHubAction) => GetEndpoints(action => action == gitHubAction).FirstOrDefault();
    }
}