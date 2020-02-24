using VXDesign.Store.DevTools.Core.Entities.GitHub.Base;

namespace VXDesign.Store.DevTools.Core.Entities.GitHub.Users.Containers
{
    public class UserItem : IGitHubEntity
    {
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public string HtmlUrl { get; set; }
        public string Name { get; set; }
    }
}