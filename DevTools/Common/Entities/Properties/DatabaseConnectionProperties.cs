using VXDesign.Store.DevTools.Common.Attributes;

namespace VXDesign.Store.DevTools.Common.Entities.Properties
{
    public class DatabaseConnectionProperties : IPropertiesMarker
    {
        [PropertyField]
        public string DataStoreConnectionString { get; set; }

        [PropertyField]
        public string LogStoreConnectionString { get; set; }
    }
}