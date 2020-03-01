using VXDesign.Store.DevTools.Common.Core.Entities.GitHub;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Settings
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