using VXDesign.Store.DevTools.Common.Enums.Operations;

namespace VXDesign.Store.DevTools.Common.Entities.Storage
{
    public class UserRoleEntity : IDataEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserPermission UserPermissions { get; set; }
        public UserRolePermission UserRolePermissions { get; set; }
    }
}