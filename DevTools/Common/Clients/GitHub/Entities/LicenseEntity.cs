using VXDesign.Store.DevTools.Common.Clients.GitHub.Base;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Entities
{
    public class LicenseEntity : IGitHubEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}