using VXDesign.Store.DevTools.Common.Entities.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class UserRoleModelExtensions
    {
        internal static UserRoleModel ToModel(this UserRoleEntity entity) => new UserRoleModel
        {
            Id = entity.Id,
            Name = entity.Name,
            UserPermissions = entity.UserPermissions,
            UserRolePermissions = entity.UserRolePermissions
        };

        internal static UserRoleEntity ToEntity(this UserRoleModel model, int? id = null) => new UserRoleEntity
        {
            Id = id ?? model.Id,
            Name = model.Name,
            UserPermissions = model.UserPermissions,
            UserRolePermissions = model.UserRolePermissions
        };
    }
}