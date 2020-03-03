namespace VXDesign.Store.DevTools.Common.Core.Entities.GitHub
{
    public class GitHubUserEntity
    {
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public string ProfileUrl { get; set; }
    }

    public class GitHubUserProfileEntity : GitHubUserEntity
    {
        public bool IsValid { get; set; }
        public string Name { get; set; }
    }
}