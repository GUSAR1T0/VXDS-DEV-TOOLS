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

    public class GitHubUserEntity
    {
        public bool IsValid { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public string ProfileUrl { get; set; }
    }
}