using VXDesign.Store.DevTools.Core.Attributes;
using VXDesign.Store.DevTools.Core.Entities.Properties;

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