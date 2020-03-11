using System.Collections.Generic;

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

    public class GitHubRepositoryFullModel : GitHubRepositoryModel
    {
        public string Description { get; set; }
        public int StargazersCount { get; set; }
        public int WatchersCount { get; set; }
        public int SubscribersCount { get; set; }
        public int OpenIssuesCount { get; set; }
        public string License { get; set; }
        public IReadOnlyDictionary<string, string> Languages { get; set; }
    }
}