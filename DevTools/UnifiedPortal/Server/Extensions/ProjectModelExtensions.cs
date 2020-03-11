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
            GitHubRepoId = entity.GitHubRepoId,
            GitHubRepository = entity.GitHubRepository.ToModel(),
            IsActive = entity.IsActive
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