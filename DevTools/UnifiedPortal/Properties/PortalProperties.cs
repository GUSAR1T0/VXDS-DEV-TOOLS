using VXDesign.Store.DevTools.Common.Containers.Properties;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.UnifiedPortal.Properties
{
    public class PortalProperties : IPropertiesMarker
    {
        [PropertyField("SYRINX")]
        public SyrinxProperties SyrinxProperties { get; set; }
    }
}