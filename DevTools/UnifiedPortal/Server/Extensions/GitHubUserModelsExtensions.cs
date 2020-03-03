using VXDesign.Store.DevTools.Common.Core.Entities.GitHub;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.GitHub;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class GitHubUserModelExtensions
    {
        internal static GitHubUserModel ToModel(this GitHubUserEntity entity) => entity != null
            ? new GitHubUserModel
            {
                Login = entity.Login,
                AvatarUrl = entity.AvatarUrl,
                ProfileUrl = entity.ProfileUrl
            }
            : null;

        internal static GitHubUserProfileModel ToModel(this GitHubUserProfileEntity entity) => entity != null
            ? new GitHubUserProfileModel
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