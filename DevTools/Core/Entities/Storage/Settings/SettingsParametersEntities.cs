using VXDesign.Store.DevTools.Core.Entities.Storage.GitHub;

namespace VXDesign.Store.DevTools.Core.Entities.Storage.Settings
{
    public class SettingsParametersItemEntity : IDataEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class SettingsParametersEntity : IDataEntity
    {
        public CodeServiceSettingsEntity CodeServiceSettings { get; set; }
    }

    public class CodeServiceSettingsEntity
    {
        public GitHubUserEntity GitHubUser { get; set; }
    }
}