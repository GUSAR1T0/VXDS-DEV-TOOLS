namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.GitHub
{
    public class GitHubRepositoryModel
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public bool Private { get; set; }
        public GitHubUserModel Owner { get; set; }
        public string RepositoryUrl { get; set; }
    }

    public class GitHubRepositoryShortModel
    {
        public long Id { get; set; }
        public string FullName { get; set; }
    }
}