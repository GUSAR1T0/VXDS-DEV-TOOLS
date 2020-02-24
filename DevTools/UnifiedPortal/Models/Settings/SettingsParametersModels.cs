using VXDesign.Store.DevTools.UnifiedPortal.Models.GitHub;

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
}