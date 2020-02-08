using VXDesign.Store.DevTools.Core.Enums.Operations;

namespace VXDesign.Store.DevTools.Core.Entities.Storage
{
    public class UserRoleEntity : IDataEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PortalPermission PortalPermissions { get; set; }
    }
}