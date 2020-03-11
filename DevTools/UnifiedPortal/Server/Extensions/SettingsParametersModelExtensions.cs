using VXDesign.Store.DevTools.Common.Core.Entities.Settings;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Settings;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class SettingsParametersModelExtensions
    {
        internal static SettingsParametersModel ToModel(this SettingsParametersEntity entity) => new SettingsParametersModel
        {
            CodeServicesSettings = entity.CodeServiceSettings.ToModel()
        };

        private static CodeServicesSettingsModel ToModel(this CodeServiceSettingsEntity entity) => new CodeServicesSettingsModel
        {
            GitHubUser = entity.GitHubUser.ToModel()
        };
    }
}