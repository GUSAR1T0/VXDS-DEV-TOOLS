using VXDesign.Store.DevTools.Core.Entities.Storage.GitHub;
using VXDesign.Store.DevTools.UnifiedPortal.Models.GitHub;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class GitHubRepositoryModelExtensions
    {
        internal static GitHubRepositoryModel ToModel(this GitHubRepositoryEntity entity) => entity != null
            ? new GitHubRepositoryModel
            {
                Id = entity.Id,
                FullName = entity.FullName,
                Private = entity.Private,
                Owner = entity.Owner.ToModel(),
                RepositoryUrl = entity.RepositoryUrl
            }
            : null;

        internal static GitHubRepositoryShortModel ToModel(this GitHubRepositoryShortEntity entity) => new GitHubRepositoryShortModel
        {
            Id = entity.Id,
            FullName = entity.FullName
        };
    }
}