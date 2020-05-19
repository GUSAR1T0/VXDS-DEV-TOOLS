using VXDesign.Store.DevTools.Common.Migrations.Camunda;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Camunda.Workflows
{
    public class DeploymentSettings : CamundaDeploymentSettings
    {
        public override string ProjectName => "SimpleNoteService";
        public override string VersionName => "1";
    }
}