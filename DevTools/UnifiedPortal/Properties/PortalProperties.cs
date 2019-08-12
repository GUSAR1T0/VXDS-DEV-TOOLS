using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.UnifiedPortal.Properties
{
    public class PortalProperties : IPropertiesMarker
    {
        [PropertyField(Key = "Syrinx")]
        public SyrinxProperties SyrinxProperties { get; set; }

        [PropertyField(Key = "AuthorizationToken")]
        public AuthorizationTokenProperties AuthorizationTokenProperties { get; set; }
    }
}