using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.UnifiedPortal.Properties
{
    public class PortalProperties : IPropertiesMarker
    {
        [PropertyField(Key = "Syrinx")]
        public SyrinxProperties SyrinxProperties { get; set; }

        [PropertyField(Key = "Database")]
        public DatabaseConnectionProperties DatabaseConnectionProperties { get; set; }
    }
}