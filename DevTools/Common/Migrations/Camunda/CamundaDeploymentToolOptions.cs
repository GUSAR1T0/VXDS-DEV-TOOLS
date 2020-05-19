using CommandLine;
using VXDesign.Store.DevTools.Common.Migrations.Common;

namespace VXDesign.Store.DevTools.Common.Migrations.Camunda
{
    public class CamundaDeploymentToolOptions
    {
        [Value(0, Required = true)]
        public MigrationAction Action { get; set; }
    }
}