namespace VXDesign.Store.DevTools.Common.Core.Entities.GitHub
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