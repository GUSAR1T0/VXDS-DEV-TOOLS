namespace VXDesign.Store.DevTools.Common.Core.Properties
{
    public class DatabaseConnectionProperties : IPropertiesMarker
    {
        [PropertyField]
        public string DataStoreConnectionString { get; set; }

        [PropertyField]
        public string LogStoreConnectionString { get; set; }
    }
}