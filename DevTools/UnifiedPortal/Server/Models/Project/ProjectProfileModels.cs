using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.GitHub;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Project
{
    public class ProjectProfileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public long? GitHubRepoId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProjectSearchModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
    }

    public class ProjectProfileGetModel : ProjectProfileModel
    {
        public bool GitHubTokenSetup { get; set; }
        public GitHubRepositoryFullModel GitHubRepository { get; set; }
    }
}