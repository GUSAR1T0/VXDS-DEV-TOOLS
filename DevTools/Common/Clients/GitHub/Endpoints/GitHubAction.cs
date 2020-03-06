using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Endpoints
{
    public enum GitHubAction
    {
        [GitHubEndpoint("Get the authenticated user", HttpMethod.Get, "user")]
        GetUser = 1,

        [GitHubEndpoint("List your repositories", HttpMethod.Get, "user/repos")]
        GetUserRepositories = 2,

        [GitHubEndpoint("Get your repository", HttpMethod.Get, "repos/{repoFullName}")]
        GetUserRepository = 3
    }
}