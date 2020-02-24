using VXDesign.Store.DevTools.Core.Entities.Storage.GitHub;
using VXDesign.Store.DevTools.UnifiedPortal.Models.GitHub;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class GitHubUserModelExtensions
    {
        internal static GitHubUserModel ToModel(this GitHubUserEntity entity) => entity != null
            ? new GitHubUserModel
            {
                IsValid = entity.IsValid,
                Login = entity.Login,
                Name = entity.Name,
                AvatarUrl = entity.AvatarUrl,
                ProfileUrl = entity.ProfileUrl
            }
            : null;
    }
}