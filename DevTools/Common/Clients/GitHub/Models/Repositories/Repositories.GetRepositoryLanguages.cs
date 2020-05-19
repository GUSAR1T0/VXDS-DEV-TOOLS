using VXDesign.Store.DevTools.Common.Clients.GitHub.Base;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Endpoints;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Models.Repositories
{
    public static partial class Repositories
    {
        public class GetRepositoryLanguagesRequest : GitHubRequest<GetRepositoryLanguagesResponse>
        {
            public override GitHubAction Action => GitHubAction.GetRepositoryLanguages;

            public readonly string owner;
            public readonly string repository;

            public GetRepositoryLanguagesRequest(string owner, string repository)
            {
                this.owner = owner;
                this.repository = repository;
            }
        }

        public class GetRepositoryLanguagesResponse : GitHubDynamicResponse
        {
        }
    }
}