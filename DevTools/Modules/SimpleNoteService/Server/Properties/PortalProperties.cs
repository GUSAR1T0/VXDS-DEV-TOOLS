using VXDesign.Store.DevTools.Common.Core.Properties;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Properties
{
    public class PortalProperties : IPropertiesMarker
    {
        [PropertyField(Key = "Syrinx")]
        public SyrinxProperties SyrinxProperties { get; set; }

        [PropertyField(Key = "UnifiedPortal")]
        public UnifiedPortalProperties UnifiedPortalProperties { get; set; }

        [PropertyField(Key = "Database")]
        public DatabaseConnectionProperties DatabaseConnectionProperties { get; set; }
    }
}