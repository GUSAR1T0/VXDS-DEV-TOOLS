using Microsoft.Extensions.DependencyInjection;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Utils;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;
using VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Host;
using VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Module;
using VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Properties;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers
{
    class Program
    {
        static void Main(string[] args) => CamundaWorkers<WorkersProperties>.Builder()
            .SetProperties(ConfigurationUtils.GetEnvironmentConfiguration)
            .SetLogger(ConfigurationUtils.GetLoggerConfiguration, "VXDS_CAM_WORK")
            .Configure(collection =>
            {
                collection.AddScoped<IModuleStore, ModuleStore>();
                collection.AddScoped<IPortalSettingsStore, PortalSettingsStore>();
            })

            #region Module

            // .AddWorker<Fixture.AfterWorker>()
            // .AddWorker<Fixture.BeforeWorker>()
            // .AddWorker<Fixture.ErrorWorker>()
            // .AddWorker<DatabaseMigration.UpgradeWorker>()
            // .AddWorker<DatabaseMigration.RollbackWorker>()
            // .AddWorker<DatabaseMigration.DowngradeWorker>()
            // .AddWorker<CamundaDeployment.UpgradeWorker>()
            // .AddWorker<CamundaDeployment.RollbackWorker>()
            // .AddWorker<CamundaDeployment.DowngradeWorker>()
            // .AddWorker<CamundaDeployment.LaunchWorker>()
            // .AddWorker<CamundaDeployment.StopWorker>()
            // .AddWorker<Application.LaunchWorker>()
            // .AddWorker<Application.StopWorker>()

            #endregion

            #region Host

            .AddWorker<HostModule.SearchWorker>()
            .AddWorker<HostModule.StopWorker>()
            .AddWorker<HostModule.CalculateWorker>()
            .AddWorker<Host.Host.DeleteWorker>()

            #endregion

            .Launch();
    }
}