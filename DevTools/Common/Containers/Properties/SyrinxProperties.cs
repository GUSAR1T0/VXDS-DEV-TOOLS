using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Containers.Properties
{
    public class SyrinxProperties : IPropertiesMarker
    {
        [PropertyField("HOST")]
        public string Host { get; set; }

        [PropertyField("API")]
        public string Api { get; set; } = "/api";

        [PropertyField("REQUEST")]
        public string Request { get; set; } = "/camunda/request";
    }
}