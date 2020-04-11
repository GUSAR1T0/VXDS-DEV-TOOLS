using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Controllers;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.Lookup
{
    public class LookupValuesModel
    {
        public int PermissionGroupId { get; set; }
        public IEnumerable<EnumModel> PortalPermissions { get; set; }
    }
}