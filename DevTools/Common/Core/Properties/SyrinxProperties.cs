namespace VXDesign.Store.DevTools.Common.Core.Properties
{
    public class SyrinxProperties : IPropertiesMarker
    {
        [PropertyField]
        public HostProperties Host { get; set; }

        [PropertyField]
        public string Api { get; set; } = "api";

        [PropertyField]
        public string CamundaRequestEndpoint { get; set; } = "camunda/request";

        [PropertyField]
        public string VerifyAuthenticationEndpoint { get; set; } = "account/verify";
    }
}