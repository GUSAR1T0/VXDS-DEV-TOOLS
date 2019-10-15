namespace VXDesign.Store.DevTools.SRS.Syrinx.Models.Authorization
{
    public class SignInModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SignUpModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Color { get; set; }
    }

    public class UserAuthorizationModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Color { get; set; }
    }
}