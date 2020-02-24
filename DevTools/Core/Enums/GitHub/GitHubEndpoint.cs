using VXDesign.Store.DevTools.Core.Attributes;
using VXDesign.Store.DevTools.Core.Enums.HTTP;

namespace VXDesign.Store.DevTools.Core.Enums.GitHub
{
    public enum GitHubEndpoint
    {
        [GitHubEndpoint("Get the authenticated user", HttpMethod.Get, "user")]
        GetUser = 1,

        [GitHubEndpoint("List your repositories", HttpMethod.Get, "user/repos")]
        GetUserRepositories = 2
    }
}