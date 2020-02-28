using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Core.Entities.Storage.Permission;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Permission;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class PermissionModelsExtensions
    {
        internal static IEnumerable<PermissionModel> ToModel(this IEnumerable<PermissionEntity> entities) => entities.Select(entity => new PermissionModel
        {
            Id = entity.Id,
            Name = entity.Name
        });

        internal static PermissionGroupModel ToModel(this PermissionGroupEntity entity) => new PermissionGroupModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Permissions = entity.Permissions.ToModel()
        };
    }
}