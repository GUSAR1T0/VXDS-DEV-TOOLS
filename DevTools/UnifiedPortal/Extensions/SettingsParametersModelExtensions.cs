using VXDesign.Store.DevTools.Core.Entities.Storage.Settings;
using VXDesign.Store.DevTools.UnifiedPortal.Models.Settings;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
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