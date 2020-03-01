using VXDesign.Store.DevTools.Common.Clients.Syrinx.Base;
using VXDesign.Store.DevTools.Common.Core.Entities.User;

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