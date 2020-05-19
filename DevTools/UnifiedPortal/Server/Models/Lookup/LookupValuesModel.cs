using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Lookup
{
    public class LookupValuesModel
    {
        public IEnumerable<EnumModel> PortalPermissions { get; set; }
        public IEnumerable<EnumModel> IncidentStatuses { get; set; }
        public IEnumerable<EnumModel> NotificationLevels { get; set; }
        public IEnumerable<EnumModel> HostOperatingSystems { get; set; }
        public IEnumerable<EnumModel> HostConnectionTypes { get; set; }
        public IEnumerable<EnumModel> ModuleStatuses { get; set; }
        public IEnumerable<EnumModel> ModuleConfigurationVerdicts { get; set; }
        public IEnumerable<EnumModel> FileExtensions { get; set; }
    }
}