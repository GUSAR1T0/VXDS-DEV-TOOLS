using System.Collections.Generic;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Dashboard
{
    public class UserRoleDataEntity : IDataEntity
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class UserRolesDataEntity
    {
        public IEnumerable<UserRoleDataEntity> UserRoles { get; set; } 
        public int Total { get; set; } 
    }
}