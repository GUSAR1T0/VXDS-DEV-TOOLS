using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Module
{
    public class ModuleModel
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Color { get; set; }

        public int HostId { get; set; }
        public string HostName { get; set; }
        public string HostDomain { get; set; }
        public HostOperatingSystem HostOperatingSystem { get; set; }

        public ModuleStatus Status { get; set; }

        public IEnumerable<ModuleConfigurationModel> Configurations { get; set; }
    }
}