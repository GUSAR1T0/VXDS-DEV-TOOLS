using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Lookup
{
    public class LookupValuesModel
    {
        public IEnumerable<EnumModel> PortalPermissions { get; set; }
        public IEnumerable<EnumModel> IncidentStatuses { get; set; }
        public IEnumerable<EnumModel> NotificationLevels { get; set; }
        public IEnumerable<EnumModel> HostOperationSystems { get; set; }
        public IEnumerable<EnumModel> HostConnectionTypes { get; set; }
    }
}