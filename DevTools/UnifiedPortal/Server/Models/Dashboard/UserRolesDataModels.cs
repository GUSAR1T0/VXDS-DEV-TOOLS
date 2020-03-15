using System.Collections.Generic;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Dashboard
{
    public class UserRoleDataModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class UserRolesDataModel
    {
        public IEnumerable<UserRoleDataModel> UserRoles { get; set; } 
        public int Total { get; set; } 
    }
}