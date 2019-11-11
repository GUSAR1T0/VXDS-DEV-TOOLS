using VXDesign.Store.DevTools.Core.Attributes;

namespace VXDesign.Store.DevTools.Core.Entities.Properties
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