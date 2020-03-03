using VXDesign.Store.DevTools.Common.Clients.GitHub.Base;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Entities
{
    public class UserEntity : IGitHubEntity
    {
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public string HtmlUrl { get; set; }
    }

    public class UserProfileEntity : UserEntity
    {
        public string Name { get; set; }
    }
}