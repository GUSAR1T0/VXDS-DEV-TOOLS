using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Utils;
using VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Properties;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers
{
    class Program
    {
        static void Main(string[] args) => CamundaWorkers<WorkersProperties>.Builder()
            .SetProperties(ConfigurationUtils.GetEnvironmentConfiguration)
            .SetLogger(ConfigurationUtils.GetLoggerConfiguration, "VXDS_CAM")
            .AddWorker<FixtureAfterWorker>()
            .AddWorker<FixtureBeforeWorker>()
            .AddWorker<FixtureErrorWorker>()
            .AddWorker<DatabaseMigrationUpgradeWorker>()
            .AddWorker<DatabaseMigrationDowngradeToPreviousWorker>()
            .AddWorker<DatabaseMigrationDowngradeWorker>()
            .AddWorker<CamundaDeploymentUpgradeWorker>()
            .AddWorker<CamundaDeploymentPreviousWorker>()
            .AddWorker<CamundaDeploymentDowngradeWorker>()
            .AddWorker<ApplicationLaunchWorker>()
            .AddWorker<ApplicationStopWorker>()
            .Launch();
    }
}