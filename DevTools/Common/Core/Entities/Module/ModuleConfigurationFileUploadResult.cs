using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Module
{
    public class ModuleConfigurationFileUploadResult
    {
        public int? ModuleId { get; set; }
        public int FileId { get; set; }
        public string Alias { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
        public string OldVersion { get; set; }
        public string NewVersion { get; set; }

        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? HostId { get; set; }
        public string HostName { get; set; }
        public string HostDomain { get; set; }
        public HostOperatingSystem? HostOperatingSystem { get; set; }

        public IEnumerable<HostOperatingSystem> OperatingSystems { get; set; }
        public ModuleConfigurationVerdict Verdict { get; set; }
    }
}