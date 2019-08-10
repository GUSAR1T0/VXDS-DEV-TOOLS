using VXDesign.Store.DevTools.Common.Entities.Properties;
using VXDesign.Store.DevTools.SRS.Camunda;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Properties
{
    public class PortalProperties : IPropertiesMarker
    {
        [PropertyField("CAMUNDA")]
        public CamundaProperties CamundaProperties { get; set; }
    }
}