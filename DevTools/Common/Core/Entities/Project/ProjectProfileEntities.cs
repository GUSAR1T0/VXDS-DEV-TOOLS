using VXDesign.Store.DevTools.Common.Core.Entities.GitHub;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Project
{
    public class ProjectProfileEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public long? GitHubRepoId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProjectProfileGetEntity : ProjectProfileEntity
    {
        public bool GitHubTokenSetup { get; set; }
        public GitHubRepositoryFullEntity GitHubRepository { get; set; }
    }
}