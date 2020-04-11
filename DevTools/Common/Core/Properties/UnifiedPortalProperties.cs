namespace VXDesign.Store.DevTools.Common.Core.Properties
{
    public class UnifiedPortalProperties : IPropertiesMarker
    {
        [PropertyField]
        public string Host { get; set; }

        [PropertyField]
        public string Api { get; set; } = "api";
    }
}