using VXDesign.Store.DevTools.Common.Enums.Operations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Models.User
{
    public class UserRoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserPermission UserPermissions { get; set; }
        public UserRolePermission UserRolePermissions { get; set; }
    }
}