using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.SRS.Camunda
{
    public class CamundaProperties : PropertiesMarker
    {
        [PropertyField("HOST")]
        public string Host { get; set; }

        [PropertyField("API", Default = "/rest/engine/default")]
        public string Api { get; set; }

        [PropertyField("LOGIN")]
        public string Login { get; set; }

        [PropertyField("PASSWORD")]
        public string Password { get; set; }
    }
}