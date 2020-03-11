using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Properties;
using VXDesign.Store.DevTools.SRS.Authentication;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Properties
{
    public class PortalProperties : IPropertiesMarker
    {
        [PropertyField(Key = "Camunda")]
        public CamundaProperties CamundaProperties { get; set; }

        [PropertyField(Key = "Database")]
        public DatabaseConnectionProperties DatabaseConnectionProperties { get; set; }

        [PropertyField(Key = "AuthenticationToken")]
        public AuthenticationTokenProperties AuthenticationTokenProperties { get; set; }
    }
}