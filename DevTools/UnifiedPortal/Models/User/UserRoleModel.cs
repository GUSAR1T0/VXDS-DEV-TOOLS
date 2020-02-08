using VXDesign.Store.DevTools.Core.Enums.Operations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Models.User
{
    public class UserRoleShortInfoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserRoleFullInfoModel : UserRoleShortInfoModel
    {
        public PortalPermission PortalPermissions { get; set; }
    }
}