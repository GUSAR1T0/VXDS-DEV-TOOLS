using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.User;

namespace VXDesign.Store.DevTools.Common.Clients.Syrinx.Base
{
    public class Permissions
    {
        public IEnumerable<UserRolePermissionEntity> ExpectedUserPermissions { get; set; }

        public bool HasPermissions(UserAuthorizationEntity entity) =>
        (
            from expectedUserPermission in ExpectedUserPermissions
            let userPermissions = entity.Permissions.FirstOrDefault(item => item.PermissionGroupId == expectedUserPermission.PermissionGroupId)
            where userPermissions != null && (expectedUserPermission.Permissions == 0 || (userPermissions.Permissions & expectedUserPermission.Permissions) != 0)
            select true
        ).Any();
    }
}