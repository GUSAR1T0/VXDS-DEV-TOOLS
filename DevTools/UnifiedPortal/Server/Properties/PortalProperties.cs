using VXDesign.Store.DevTools.Common.Core.Properties;
using VXDesign.Store.DevTools.Core.Attributes;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Properties
{
    public class PortalProperties : IPropertiesMarker
    {
        [PropertyField(Key = "Syrinx")]
        public SyrinxProperties SyrinxProperties { get; set; }

        [PropertyField(Key = "Database")]
        public DatabaseConnectionProperties DatabaseConnectionProperties { get; set; }
    }
}