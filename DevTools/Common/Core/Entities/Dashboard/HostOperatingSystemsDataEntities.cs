using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Dashboard
{
    public class HostOperatingSystemDataEntity : IDataEntity
    {
        public HostOperatingSystem OperatingSystem { get; set; }
        public int Count { get; set; }
    }

    public class HostOperatingSystemsDataEntity
    {
        public IEnumerable<HostOperatingSystemDataEntity> OperatingSystems { get; set; } 
        public int Total { get; set; } 
    }
}