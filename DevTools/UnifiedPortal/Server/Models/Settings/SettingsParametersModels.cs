using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.GitHub;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Settings
{
    public class SettingsParametersModel
    {
        public CodeServicesSettingsModel CodeServicesSettings { get; set; }
    }

    public class CodeServicesSettingsModel
    {
        public GitHubUserProfileModel GitHubUser { get; set; }
    }
}