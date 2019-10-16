namespace VXDesign.Store.DevTools.Common.DataStorage.Entities
{
    public class UserAuthorizationEntity : IDataEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Color { get; set; }
    }

    public class UserRegistrationEntity : UserAuthorizationEntity
    {
        public string Password { get; set; }
    }

    public class UserProfileEntity : UserAuthorizationEntity
    {
        public string Location { get; set; }
        public string Bio { get; set; }
        public string RoleId { get; set; }
    }
}