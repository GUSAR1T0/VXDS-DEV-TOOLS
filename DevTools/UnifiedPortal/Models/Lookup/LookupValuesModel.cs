using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Entities.Controllers;

namespace VXDesign.Store.DevTools.UnifiedPortal.Models.Lookup
{
    public class LookupValuesModel
    {
        public IEnumerable<EnumModel> UserRolePermissions { get; set; }
        public IEnumerable<EnumModel> UserPermissions { get; set; }
    }
}