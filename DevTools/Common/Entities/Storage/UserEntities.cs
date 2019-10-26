using VXDesign.Store.DevTools.Common.Enums.Operations;

namespace VXDesign.Store.DevTools.Common.Entities.Storage
{
    public class UserEntity : IDataEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Color { get; set; }
    }

    public class UserListItem : UserEntity
    {
        public string UserRole { get; set; }
        public bool IsActivated { get; set; }
    }

    public class UserAuthorizationEntity : UserEntity
    {
        public UserPermission UserPermissions { get; set; } = 0;
        public UserRolePermission UserRolePermissions { get; set; } = 0;
    }

    public class UserRegistrationEntity : UserAuthorizationEntity
    {
        public string Password { get; set; }
    }

    public class UserProfileEntity : UserEntity
    {
        public string Location { get; set; }
        public string Bio { get; set; }
        public int? UserRoleId { get; set; }
        public UserRoleEntity UserRole { get; set; }
        public bool IsActivated { get; set; }
    }
}