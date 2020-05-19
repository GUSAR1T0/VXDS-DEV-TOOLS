using VXDesign.Store.DevTools.Common.Core.Properties;

namespace VXDesign.Store.DevTools.UnifiedPortal.Database.Properties
{
    public class ProjectProperties : IPropertiesMarker
    {
        [PropertyField(Key = "Database")]
        public DatabaseConnectionProperties DatabaseConnectionProperties { get; set; }
    }
}