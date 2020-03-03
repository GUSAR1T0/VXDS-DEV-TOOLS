using VXDesign.Store.DevTools.Common.Clients.GitHub.Base;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Entities;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Models.Users
{
    public partial class Users
    {
        public class GetUserRequest : GitHubRequest<GetUserResponse>
        {
            public override GitHubAction Action => GitHubAction.GetUser;
        }

        public class GetUserResponse : GitHubSingleResponse<UserProfileEntity>
        {
        }
    }
}