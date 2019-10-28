using VXDesign.Store.DevTools.Common.Entities.Storage;
using VXDesign.Store.DevTools.Common.Enums.Operations;

namespace VXDesign.Store.DevTools.Common.Entities.Operations
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