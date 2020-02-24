using VXDesign.Store.DevTools.Core.Entities.GitHub.Repositories.Containers;

namespace VXDesign.Store.DevTools.Core.Entities.Storage.GitHub
{
    public class GitHubRepositoryEntity
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public bool Private { get; set; }
        public GitHubUserEntity Owner { get; set; }
        public string RepositoryUrl { get; set; }

        public static GitHubRepositoryEntity Transform(RepositoryEntity entity) => entity != null
            ? new GitHubRepositoryEntity
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Private = entity.Private,
                Owner = new GitHubUserEntity
                {
                    IsValid = true,
                    Login = entity.Owner.Login,
                    Name = entity.Owner.Name,
                    AvatarUrl = entity.Owner.AvatarUrl,
                    ProfileUrl = entity.Owner.HtmlUrl
                },
                RepositoryUrl = entity.HtmlUrl
            }
            : null;
    }

    public class GitHubRepositoryShortEntity
    {
        public long Id { get; set; }
        public string FullName { get; set; }
    }
}