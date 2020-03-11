using System.Collections.Generic;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Permission
{
    public class PermissionModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class PermissionGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PermissionModel> Permissions { get; set; }
    }
}