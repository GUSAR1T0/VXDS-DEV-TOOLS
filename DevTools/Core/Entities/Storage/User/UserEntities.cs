using VXDesign.Store.DevTools.Core.Enums.Operations;

namespace VXDesign.Store.DevTools.Core.Entities.Storage.User
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
        public PortalPermission PortalPermissions { get; set; } = 0;
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