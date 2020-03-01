using VXDesign.Store.DevTools.Common.Clients.GitHub.Base;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Entities
{
    public class UserItem : IGitHubEntity
    {
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public string HtmlUrl { get; set; }
        public string Name { get; set; }
    }
}