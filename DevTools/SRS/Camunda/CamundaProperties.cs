using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.SRS.Camunda
{
    public class CamundaProperties : IPropertiesMarker
    {
        [PropertyField("HOST")]
        public string Host { get; set; }

        [PropertyField("API")]
        public string Api { get; set; } = "/rest/engine/default";

        [PropertyField("LOGIN")]
        public string Login { get; set; }

        [PropertyField("PASSWORD")]
        public string Password { get; set; }
    }
}