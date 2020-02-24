using VXDesign.Store.DevTools.Core.Entities.GitHub.Base;
using VXDesign.Store.DevTools.Core.Entities.GitHub.Users.Containers;

namespace VXDesign.Store.DevTools.Core.Entities.GitHub.Repositories.Containers
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