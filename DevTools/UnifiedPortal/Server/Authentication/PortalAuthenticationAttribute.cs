using VXDesign.Store.DevTools.Core.Attributes;
using VXDesign.Store.DevTools.Core.Entities.Storage.User;
using VXDesign.Store.DevTools.Core.Enums.Operations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication
{
    public class PortalAuthenticationAttribute : SyrinxVerifiedAuthenticationAttribute
    {
        public PortalAuthenticationAttribute(PortalPermission portalPermissions = 0) : base(new[]
        {
            new UserRolePermissionEntity
            {
                PermissionGroupId = 1,
                Permissions = (long) portalPermissions
            }
        })
        {
        }
    }
}