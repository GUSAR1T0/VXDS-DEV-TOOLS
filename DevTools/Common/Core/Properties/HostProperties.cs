namespace VXDesign.Store.DevTools.Common.Core.Properties
{
    public class HostProperties : IPropertiesMarker
    {
        [PropertyField]
        public string Internal { get; set; }

        [PropertyField]
        public string External { get; set; }

        public string GetExternalAddress() => string.IsNullOrWhiteSpace(External) ? Internal : External;
    }
}