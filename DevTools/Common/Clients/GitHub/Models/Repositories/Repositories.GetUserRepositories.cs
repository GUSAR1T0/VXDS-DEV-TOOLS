using VXDesign.Store.DevTools.Common.Clients.GitHub.Base;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Entities;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Models.Repositories
{
    public partial class Repositories
    {
        public class GetUserRepositoriesRequest : GitHubRequest<GetUserRepositoriesResponse>
        {
            public override GitHubAction Action => GitHubAction.GetUserRepositories;
        }

        public class GetUserRepositoriesResponse : GitHubMultipleResponse<RepositoryListItemEntity>
        {
        }
    }
}