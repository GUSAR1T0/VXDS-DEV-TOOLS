namespace VXDesign.Store.DevTools.Core.Entities.Storage.GitHub
{
    public class GitHubUserEntity
    {
        public bool IsValid { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public string ProfileUrl { get; set; }
    }
}