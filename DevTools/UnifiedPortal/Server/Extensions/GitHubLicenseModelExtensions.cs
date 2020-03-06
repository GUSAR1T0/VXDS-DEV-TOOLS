using VXDesign.Store.DevTools.Common.Core.Entities.GitHub;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.GitHub;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class GitHubLicenseModelExtensions
    {
        internal static GitHubLicenseModel ToModel(this GitHubLicenseEntity entity) => entity != null
            ? new GitHubLicenseModel
            {
                Name = entity.Name,
                Url = entity.Url
            }
            : null;
    }
}