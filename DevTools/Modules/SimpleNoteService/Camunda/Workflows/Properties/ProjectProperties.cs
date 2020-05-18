using VXDesign.Store.DevTools.Common.Core.Properties;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Camunda.Workflows.Properties
{
    public class ProjectProperties : IPropertiesMarker
    {
        [PropertyField(Key = "Syrinx")]
        public SyrinxProperties SyrinxProperties { get; set; }

        [PropertyField(Key = "Database")]
        public DatabaseConnectionProperties DatabaseConnectionProperties { get; set; }
    }
}