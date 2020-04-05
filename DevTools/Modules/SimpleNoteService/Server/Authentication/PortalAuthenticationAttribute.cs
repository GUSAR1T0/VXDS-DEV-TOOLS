using VXDesign.Store.DevTools.Common.Clients.Syrinx.Base;
using VXDesign.Store.DevTools.Common.Core.Entities.User;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Constants;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Authentication
{
    public class PortalAuthenticationAttribute : SyrinxVerifiedAuthenticationAttribute
    {
        public PortalAuthenticationAttribute(PortalPermission portalPermissions = 0) : base(new[]
        {
            new UserRolePermissionEntity
            {
                PermissionGroupId = PortalPermissionKey.PermissionGroupId,
                Permissions = (long) portalPermissions
            }
        })
        {
        }
    }
}