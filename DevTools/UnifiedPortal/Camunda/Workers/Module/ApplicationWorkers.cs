using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Module
{
    public static class Application
    {
        [CamundaWorkerTopic("UnifiedPortal.Module.Application.Launch")]
        public class LaunchWorker : ModuleProcessingWorker
        {
            public LaunchWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore) : base(fileStore, moduleStore, portalSettingsStore)
            {
            }

            protected override void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client)
            {
                SendCommands(operation, logger, client, instructions.Application.Launch);
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Module.Application.Stop")]
        public class StopWorker : ModuleProcessingWorker
        {
            protected override bool Regression => true;

            public StopWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore) : base(fileStore, moduleStore, portalSettingsStore)
            {
            }

            protected override void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client)
            {
                SendCommands(operation, logger, client, instructions.Application.Stop);
            }
        }
    }
}