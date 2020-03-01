using System.Collections.Generic;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Permission
{
    public class PermissionEntity : IDataEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class PermissionExtendedEntity : PermissionEntity
    {
        public int PermissionGroupId { get; set; }
    }

    public class PermissionGroupShortEntity : IDataEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PermissionGroupEntity : PermissionGroupShortEntity
    {
        public IEnumerable<PermissionEntity> Permissions { get; set; }
    }
}