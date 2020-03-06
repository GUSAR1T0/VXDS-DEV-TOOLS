namespace VXDesign.Store.DevTools.Common.Core.Entities.GitHub
{
    public class GitHubRepositoryShortEntity
    {
        public long Id { get; set; }
        public string FullName { get; set; }
    }

    public class GitHubRepositoryEntity : GitHubRepositoryShortEntity
    {
        public bool Private { get; set; }
        public GitHubUserEntity Owner { get; set; }
        public string RepositoryUrl { get; set; }
    }

    public class GitHubRepositoryFullEntity : GitHubRepositoryEntity
    {
        public string Description { get; set; }
        public int StargazersCount { get; set; }
        public int WatchersCount { get; set; }
        public int SubscribersCount { get; set; }
        public int OpenIssuesCount { get; set; }
        public GitHubLicenseEntity License { get; set; }
    }
}