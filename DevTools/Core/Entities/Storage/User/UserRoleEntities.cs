using System.Collections.Generic;

namespace VXDesign.Store.DevTools.Core.Entities.Storage.User
{
    public class UserRoleEntity : IDataEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
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
}