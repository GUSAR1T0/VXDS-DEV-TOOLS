using VXDesign.Store.DevTools.Core.Entities.Storage;
using VXDesign.Store.DevTools.Core.Enums.Operations;

namespace VXDesign.Store.DevTools.Core.Entities.Operations
{
    public class Permissions
    {
        public UserPermission UserPermissions { get; set; }

        public bool HasPermissions(UserAuthorizationEntity entity)
        {
            return (UserPermissions == 0 || (entity.UserPermissions & UserPermissions) != 0);
        }
    }
}