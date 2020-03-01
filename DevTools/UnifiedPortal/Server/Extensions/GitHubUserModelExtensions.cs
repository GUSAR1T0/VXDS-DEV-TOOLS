using VXDesign.Store.DevTools.Common.Core.Entities.GitHub;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.GitHub;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
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