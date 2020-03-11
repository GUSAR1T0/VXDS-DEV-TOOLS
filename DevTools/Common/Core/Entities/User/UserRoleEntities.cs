using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.SSP;

namespace VXDesign.Store.DevTools.Common.Core.Entities.User
{
    public class UserRoleEntity : IDataEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserRoleExtendedEntity : UserRoleEntity
    {
        public int UserCount { get; set; }
    }

    public class UserRolePermissionEntity : IDataEntity
    {
        public int PermissionGroupId { get; set; }
        public long Permissions { get; set; }
    }

    public class UserRolePermissionExtendedEntity : UserRolePermissionEntity
    {
        public int UserRoleId { get; set; }
    }

    public class UserRoleWithPermissionsEntity : UserRoleEntity
    {
        public IEnumerable<UserRolePermissionEntity> Permissions { get; set; }
    }

    public class UserRoleListItem : IPagingResponseItemEntity
    {
        public UserRoleExtendedEntity UserRole { get; set; }
        public IEnumerable<UserRolePermissionEntity> Permissions { get; set; }
    }

    public class UserRolePagingFilter : IPagingFilterEntity
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> UserRoleNames { get; set; }
    }

    public class UserRolePagingRequest : ServerSidePagingRequest<UserRolePagingFilter>
    {
    }

    public class UserRolePagingResponse : ServerSidePagingResponse<UserRoleListItem>
    {
    }
}