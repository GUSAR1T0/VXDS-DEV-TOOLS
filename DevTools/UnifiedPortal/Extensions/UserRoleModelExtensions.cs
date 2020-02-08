using VXDesign.Store.DevTools.Core.Entities.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class UserRoleModelExtensions
    {
        internal static UserRoleFullInfoModel ToFullInfoModel(this UserRoleEntity entity) => new UserRoleFullInfoModel
        {
            Id = entity.Id,
            Name = entity.Name,
            PortalPermissions = entity.PortalPermissions
        };

        internal static UserRoleShortInfoModel ToShortInfoModel(this UserRoleEntity entity) => new UserRoleShortInfoModel
        {
            Id = entity.Id,
            Name = entity.Name
        };

        internal static UserRoleEntity ToEntity(this UserRoleFullInfoModel model, int? id = null) => new UserRoleEntity
        {
            Id = id ?? model.Id,
            Name = model.Name,
            PortalPermissions = model.PortalPermissions
        };
    }
}