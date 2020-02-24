using VXDesign.Store.DevTools.Core.Entities.GitHub.Base;
using VXDesign.Store.DevTools.Core.Entities.GitHub.Repositories.Containers;
using GitHubEndpoint = VXDesign.Store.DevTools.Core.Enums.GitHub.GitHubEndpoint;

namespace VXDesign.Store.DevTools.Core.Entities.GitHub.Repositories.Models
{
    public partial class Repositories
    {
        public class GetUserRepositoriesRequest : GitHubRequest<GetUserRepositoriesResponse>
        {
            public override GitHubEndpoint Endpoint => GitHubEndpoint.GetUserRepositories;
        }

        public class GetUserRepositoriesResponse : GitHubMultipleResponse<RepositoryEntity>
        {
        }
    }
}