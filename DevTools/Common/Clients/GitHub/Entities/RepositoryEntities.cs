using VXDesign.Store.DevTools.Common.Clients.GitHub.Base;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Entities
{
    public class RepositoryListItemEntity : IGitHubEntity
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public bool Private { get; set; }
        public UserEntity Owner { get; set; }
        public string HtmlUrl { get; set; }
    }
}