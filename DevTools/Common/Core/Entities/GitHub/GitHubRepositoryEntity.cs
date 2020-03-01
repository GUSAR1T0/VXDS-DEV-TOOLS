namespace VXDesign.Store.DevTools.Common.Core.Entities.GitHub
{
    public class GitHubRepositoryEntity
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public bool Private { get; set; }
        public GitHubUserEntity Owner { get; set; }
        public string RepositoryUrl { get; set; }
    }

    public class GitHubRepositoryShortEntity
    {
        public long Id { get; set; }
        public string FullName { get; set; }
    }
}