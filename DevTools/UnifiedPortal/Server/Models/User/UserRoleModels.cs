using System.Collections.Generic;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.User
{
    public class UserRoleShortInfoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserRolePermissionModel
    {
        public int PermissionGroupId { get; set; }
        public IEnumerable<long> Permissions { get; set; }
    }

    public class UserRoleFullInfoModel : UserRoleShortInfoModel
    {
        public IEnumerable<UserRolePermissionModel> Permissions { get; set; }
    }
}