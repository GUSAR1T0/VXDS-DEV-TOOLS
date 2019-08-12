namespace VXDesign.Store.DevTools.Common.Entities.Properties
{
    public class SyrinxProperties : IPropertiesMarker
    {
        [PropertyField]
        public string Host { get; set; }

        [PropertyField]
        public string Api { get; set; } = "/api";

        [PropertyField]
        public string Request { get; set; } = "/camunda/request";
    }
}