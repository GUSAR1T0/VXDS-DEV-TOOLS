using VXDesign.Store.DevTools.Common.Core.Entities.Project;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Project;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class ProjectModelExtensions
    {
        internal static ProjectProfileGetModel ToModel(this ProjectProfileGetEntity entity) => new ProjectProfileGetModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Alias = entity.Alias,
            Description = entity.Description,
            GitHubTokenSetup = entity.GitHubTokenSetup,
            GitHubRepoId = entity.GitHubRepoId,
            GitHubRepository = entity.GitHubRepository.ToModel(),
            IsActive = entity.IsActive
        };

        internal static ProjectSearchModel ToModel(this ProjectSearchEntity entity) => new ProjectSearchModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Alias = entity.Alias
        };

        internal static ProjectProfileEntity ToEntity(this ProjectProfileModel model) => new ProjectProfileEntity
        {
            Id = model.Id,
            Name = model.Name,
            Alias = model.Alias,
            Description = model.Description,
            GitHubRepoId = model.GitHubRepoId,
            IsActive = model.IsActive
        };
    }
}