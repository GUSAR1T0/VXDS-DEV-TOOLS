using VXDesign.Store.DevTools.Common.Core.Entities.GitHub;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.GitHub;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
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