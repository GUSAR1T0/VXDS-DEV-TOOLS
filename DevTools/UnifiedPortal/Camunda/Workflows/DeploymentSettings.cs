using VXDesign.Store.DevTools.Common.Migrations.Camunda;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workflows
{
    public class DeploymentSettings : CamundaDeploymentSettings
    {
        public override string ProjectName => "UnifiedPortal";
        public override string VersionName => "1";
    }
}