namespace VXDesign.Store.DevTools.UnifiedPortal.Models.User
{
    public class UserProfileGetModel
    {
        #region General Info

        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Color { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }

        #endregion

        #region System Info

        public UserRoleFullInfoModel UserRole { get; set; }
        public bool IsActivated { get; set; }

        #endregion
    }

    public class UserProfileGeneralInfoUpdateModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Color { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
    }

    public class UserProfileAccountSpecificInfoUpdateModel
    {
        public int? UserRoleId { get; set; }
        public bool IsActivated { get; set; }
    }
}