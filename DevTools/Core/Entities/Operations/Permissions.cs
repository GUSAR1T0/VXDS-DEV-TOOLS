using VXDesign.Store.DevTools.Core.Entities.Storage.User;
using VXDesign.Store.DevTools.Core.Enums.Operations;

namespace VXDesign.Store.DevTools.Core.Entities.Operations
{
    public class Permissions
    {
        public PortalPermission PortalPermissions { get; set; }

        public bool HasPermissions(UserAuthorizationEntity entity)
        {
            return (PortalPermissions == 0 || (entity.PortalPermissions & PortalPermissions) != 0);
        }
    }
}