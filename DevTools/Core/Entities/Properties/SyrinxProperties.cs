using VXDesign.Store.DevTools.Core.Attributes;

namespace VXDesign.Store.DevTools.Core.Entities.Properties
{
    public class SyrinxProperties : IPropertiesMarker
    {
        [PropertyField]
        public string Host { get; set; }

        [PropertyField]
        public string Api { get; set; } = "api";

        [PropertyField]
        public string CamundaRequestEndpoint { get; set; } = "camunda/request";

        [PropertyField]
        public string VerifyAuthenticationEndpoint { get; set; } = "account/verify";
    }
}