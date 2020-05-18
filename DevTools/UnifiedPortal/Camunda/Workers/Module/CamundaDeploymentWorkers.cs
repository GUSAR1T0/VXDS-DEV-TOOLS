using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Module
{
    public static class CamundaDeployment
    {
        [CamundaWorkerTopic("UnifiedPortal.Module.Camunda.Upgrade")]
        public class UpgradeWorker : ModuleProcessingWorker
        {
            public UpgradeWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore) : base(fileStore, moduleStore, portalSettingsStore)
            {
            }

            protected override void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client)
            {
                SendCommands(operation, logger, client, instructions.Camunda?.Workflows?.Upgrade);
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Module.Camunda.Rollback")]
        public class RollbackWorker : ModuleProcessingWorker
        {
            protected override bool Regression => true;

            public RollbackWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore) : base(fileStore, moduleStore, portalSettingsStore)
            {
            }

            protected override void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client)
            {
                SendCommands(operation, logger, client, instructions.Camunda?.Workflows?.Rollback);
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Module.Camunda.Downgrade")]
        public class DowngradeWorker : ModuleProcessingWorker
        {
            protected override bool Regression => true;

            public DowngradeWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore) : base(fileStore, moduleStore, portalSettingsStore)
            {
            }

            protected override void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client)
            {
                SendCommands(operation, logger, client, instructions.Camunda?.Workflows?.Downgrade);
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Module.Camunda.Launch")]
        public class LaunchWorker : ModuleProcessingWorker
        {
            public LaunchWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore) : base(fileStore, moduleStore, portalSettingsStore)
            {
            }

            protected override void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client)
            {
                SendCommands(operation, logger, client, instructions.Camunda?.Workers?.Launch);
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Module.Camunda.Stop")]
        public class StopWorker : ModuleProcessingWorker
        {
            protected override bool Regression => true;

            public StopWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore) : base(fileStore, moduleStore, portalSettingsStore)
            {
            }

            protected override void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client)
            {
                SendCommands(operation, logger, client, instructions.Camunda?.Workers?.Stop);
            }
        }
    }
}