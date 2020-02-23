using VXDesign.Store.DevTools.Core.Entities.GitHub.Base;
using VXDesign.Store.DevTools.Core.Entities.GitHub.Users.Containers;
using GitHubEndpoint = VXDesign.Store.DevTools.Core.Enums.GitHub.GitHubEndpoint;

namespace VXDesign.Store.DevTools.Core.Entities.GitHub.Users.Models
{
    public partial class Users
    {
        public class GetUserRequest : GitHubRequest<GetUserResponse>
        {
            public override GitHubEndpoint Endpoint => GitHubEndpoint.GetUser;
        }

        public class GetUserResponse : GitHubSingleResponse<GetUserResult>
        {
        }
    }
}