using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Containers.Properties
{
    public class SyrinxProperties : IPropertiesMarker
    {
        [PropertyField]
        public string Host { get; set; }

        [PropertyField]
        public string Api { get; set; } = "api";

        [PropertyField]
        public string CamundaRequestEndpoint { get; set; } = "camunda/request";
    }
}