using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Dashboard
{
    public class HostOperatingSystemDataModel
    {
        public HostOperatingSystem OperatingSystem { get; set; }
        public int Count { get; set; }
    }

    public class HostOperatingSystemsDataModel
    {
        public IEnumerable<HostOperatingSystemDataModel> OperatingSystems { get; set; } 
        public int Total { get; set; } 
    }
}