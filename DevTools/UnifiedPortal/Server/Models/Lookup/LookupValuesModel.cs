using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Controllers;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Lookup
{
    public class LookupValuesModel
    {
        public IEnumerable<EnumModel> PortalPermissions { get; set; }
        public IEnumerable<EnumModel> IncidentStatuses { get; set; }
        public IEnumerable<EnumModel> NotificationLevels { get; set; }
    }
}