namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.GitHub
{
    public class GitHubRepositoryShortModel
    {
        public long Id { get; set; }
        public string FullName { get; set; }
    }

    public class GitHubRepositoryModel : GitHubRepositoryShortModel
    {
        public bool Private { get; set; }
        public GitHubUserModel Owner { get; set; }
        public string RepositoryUrl { get; set; }
    }
}