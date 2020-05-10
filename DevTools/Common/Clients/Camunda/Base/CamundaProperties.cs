using VXDesign.Store.DevTools.Common.Core.Properties;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Base
{
    public class CamundaProperties : IPropertiesMarker
    {
        [PropertyField]
        public string Host { get; set; }

        [PropertyField]
        public string Api { get; set; } = "engine-rest";

        [PropertyField]
        public string Login { get; set; }

        [PropertyField]
        public string Password { get; set; }
    }
}