using System;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.User;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.User;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class UserRoleModelExtensions
    {
        internal static UserRolePermissionModel ToModel(this UserRolePermissionEntity entity) => new UserRolePermissionModel
        {
            PermissionGroupId = entity.PermissionGroupId,
            Permissions = Convert.ToString(entity.Permissions, 2)
                .ToCharArray()
                .Reverse()
                .Select((symbol, index) => new { symbol, index })
                .Where(item => item.symbol == '1')
                .Select(item => (long) Math.Pow(2, item.index))
        };

        internal static UserRoleFullInfoModel ToFullInfoModel(this UserRoleWithPermissionsEntity entity) => new UserRoleFullInfoModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Permissions = entity.Permissions.Select(item => item.ToModel())
        };

        internal static UserRoleListItemModel ToModel(this UserRoleListItem entity) => new UserRoleListItemModel
        {
            Id = entity.UserRole.Id,
            Name = entity.UserRole.Name,
            UserCount = entity.UserRole.UserCount,
            Permissions = entity.Permissions.Select(item => item.ToModel()),
        };

        internal static UserRoleShortInfoModel ToShortInfoModel(this UserRoleEntity entity) => new UserRoleShortInfoModel
        {
            Id = entity.Id,
            Name = entity.Name
        };

        internal static UserRoleWithPermissionsEntity ToEntity(this UserRoleFullInfoModel model, int? id = null) => new UserRoleWithPermissionsEntity
        {
            Id = id ?? model.Id,
            Name = model.Name,
            Permissions = model.Permissions.Select(item => new UserRolePermissionEntity
            {
                PermissionGroupId = item.PermissionGroupId,
                Permissions = item.Permissions.Sum()
            })
        };
    }
}