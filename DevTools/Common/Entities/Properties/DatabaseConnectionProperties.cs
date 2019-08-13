namespace VXDesign.Store.DevTools.Common.Entities.Properties
{
    public class DatabaseConnectionProperties : IPropertiesMarker
    {
        [PropertyField]
        public string ConnectionString { get; set; }

        [PropertyField]
        public string Database { get; set; }
    }
}