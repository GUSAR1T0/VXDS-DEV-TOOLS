using VXDesign.Store.DevTools.Core.Attributes;

namespace VXDesign.Store.DevTools.Core.Entities.Properties
{
    public class DatabaseConnectionProperties : IPropertiesMarker
    {
        [PropertyField]
        public string DataStoreConnectionString { get; set; }

        [PropertyField]
        public string LogStoreConnectionString { get; set; }
    }
}