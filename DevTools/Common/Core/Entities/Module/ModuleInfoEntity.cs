using VXDesign.Store.DevTools.Common.Core.Entities.Settings;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Module
{
    public class ModuleInfoEntity : IDataEntity
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int HostId { get; set; }
        public string HostName { get; set; }
        public string HostDomain { get; set; }
        public HostOperatingSystem HostOperatingSystem { get; set; }

        public ModuleStatus Status { get; set; }
    }
}