using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Module
{
    public static class DatabaseMigration
    {
        [CamundaWorkerTopic("UnifiedPortal.Module.Database.Upgrade")]
        public class UpgradeWorker : ModuleProcessingWorker
        {
            public UpgradeWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore) : base(fileStore, moduleStore, portalSettingsStore)
            {
            }

            protected override void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client)
            {
                SendCommands(operation, logger, client, instructions.Database?.Upgrade);
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Module.Database.Rollback")]
        public class RollbackWorker : ModuleProcessingWorker
        {
            protected override bool Regression => true;

            public RollbackWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore) : base(fileStore, moduleStore, portalSettingsStore)
            {
            }

            protected override void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client)
            {
                SendCommands(operation, logger, client, instructions.Database?.Rollback);
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Module.Database.Downgrade")]
        public class DowngradeWorker : ModuleProcessingWorker
        {
            protected override bool Regression => true;

            public DowngradeWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore) : base(fileStore, moduleStore, portalSettingsStore)
            {
            }

            protected override void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client)
            {
                SendCommands(operation, logger, client, instructions.Database?.Downgrade);
            }
        }
    }
}