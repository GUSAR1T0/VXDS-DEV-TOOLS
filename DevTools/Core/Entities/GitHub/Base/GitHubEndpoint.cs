using System;
using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Core.Attributes;
using VXDesign.Store.DevTools.Core.Enums.HTTP;
using VXDesign.Store.DevTools.Core.Extensions.Base;
using GitHubEndpointEnum = VXDesign.Store.DevTools.Core.Enums.GitHub.GitHubEndpoint;

namespace VXDesign.Store.DevTools.Core.Entities.GitHub.Base
{
    public class GitHubEndpoint
    {
        public int ActionCode { get; }
        public string ActionName { get; }
        public HttpMethod Method { get; }
        public string Path { get; }

        private GitHubEndpoint(GitHubEndpointEnum gitHubEndpoint, GitHubEndpointAttribute gitHubEndpointAttribute)
        {
            ActionCode = (int) gitHubEndpoint;
            ActionName = gitHubEndpointAttribute.Description;
            Method = gitHubEndpointAttribute.Method;
            Path = '/' + gitHubEndpointAttribute.Path;
        }

        private static IEnumerable<GitHubEndpoint> GetEndpoints(Func<GitHubEndpointEnum, bool> predicate = null)
        {
            return
                from endpoint in EnumExtensions.GetValues<GitHubEndpointEnum>().Where(predicate ?? (action => true))
                let attribute = endpoint.GetAttributeOfType<GitHubEndpointAttribute>()
                select new GitHubEndpoint(endpoint, attribute);
        }

        public static IEnumerable<GitHubEndpoint> GetAll() => GetEndpoints().ToList();

        public static GitHubEndpoint GetEndpoint(GitHubEndpointEnum gitHubEndpoint) => GetEndpoints(action => action == gitHubEndpoint).FirstOrDefault();
    }
}