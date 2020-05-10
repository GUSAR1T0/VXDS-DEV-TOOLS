namespace VXDesign.Store.DevTools.Common.Migrations.Camunda
{
    public abstract class CamundaDeploymentSettings
    {
        public abstract string ProjectName { get; }
        public abstract string VersionName { get; }
    }
}