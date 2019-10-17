using VXDesign.Store.DevTools.Common.Attributes;

namespace VXDesign.Store.DevTools.Common.Entities.Properties
{
    public class CamundaProperties : IPropertiesMarker
    {
        [PropertyField]
        public string Host { get; set; }

        [PropertyField]
        public string Api { get; set; } = "rest/engine/default";

        [PropertyField]
        public string Login { get; set; }

        [PropertyField]
        public string Password { get; set; }
    }
}