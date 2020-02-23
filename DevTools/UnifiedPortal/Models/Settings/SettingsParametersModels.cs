namespace VXDesign.Store.DevTools.UnifiedPortal.Models.Settings
{
    public class SettingsParametersModel
    {
        public CodeServicesSettingsModel CodeServicesSettings { get; set; }
    }

    public class CodeServicesSettingsModel
    {
        public GitHubUserModel GitHubUser { get; set; }
    }

    public class GitHubUserModel
    {
        public bool IsValid { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public string ProfileUrl { get; set; }
    }
}