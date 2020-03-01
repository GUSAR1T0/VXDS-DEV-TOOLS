using VXDesign.Store.DevTools.Common.Clients.GitHub.Base;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Entities
{
    public class RepositoryEntity : IGitHubEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool Private { get; set; }
        public UserItem Owner { get; set; }
        public string HtmlUrl { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
    }
}