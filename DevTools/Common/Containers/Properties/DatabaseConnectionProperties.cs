using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Containers.Properties
{
    public class DatabaseConnectionProperties : IPropertiesMarker
    {
        [PropertyField]
        public string ConnectionString { get; set; }

        [PropertyField]
        public string Database { get; set; }
    }
}