namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.GitHub
{
    public class GitHubUserModel
    {
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public string ProfileUrl { get; set; }
    }

    public class GitHubUserProfileModel : GitHubUserModel
    {
        public bool IsValid { get; set; }
        public string Name { get; set; }
    }
}