using VXDesign.Store.DevTools.Common.Enums.Operations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Models.User
{
    public class UserRoleShortInfoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserRoleFullInfoModel : UserRoleShortInfoModel
    {
        public UserPermission UserPermissions { get; set; }
    }
}