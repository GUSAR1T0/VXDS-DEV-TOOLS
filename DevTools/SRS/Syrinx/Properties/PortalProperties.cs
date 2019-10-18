using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Properties;
using VXDesign.Store.DevTools.SRS.Authorization;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Properties
{
    public class PortalProperties : IPropertiesMarker
    {
        [PropertyField(Key = "Camunda")]
        public CamundaProperties CamundaProperties { get; set; }

        [PropertyField(Key = "Database")]
        public DatabaseConnectionProperties DatabaseConnectionProperties { get; set; }

        [PropertyField(Key = "AuthorizationToken")]
        public AuthorizationTokenProperties AuthorizationTokenProperties { get; set; }
    }
}