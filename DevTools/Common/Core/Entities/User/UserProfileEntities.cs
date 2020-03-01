using System.Collections.Generic;

namespace VXDesign.Store.DevTools.Common.Core.Entities.User
{
    public class UserEntity : IDataEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Color { get; set; }
    }

    public class UserAuthorizationShortEntity : UserEntity
    {
        public int UserRoleId { get; set; }
    }

    public class UserAuthorizationEntity : UserAuthorizationShortEntity
    {
        public IEnumerable<UserRolePermissionEntity> Permissions { get; set; }
    }

    public class UserRegistrationEntity : UserEntity
    {
        public string Password { get; set; }
    }

    public class UserProfileEntity : UserEntity
    {
        public string Location { get; set; }
        public string Bio { get; set; }
        public int? UserRoleId { get; set; }
        public UserRoleWithPermissionsEntity UserRole { get; set; }
        public bool IsActivated { get; set; }
    }
}