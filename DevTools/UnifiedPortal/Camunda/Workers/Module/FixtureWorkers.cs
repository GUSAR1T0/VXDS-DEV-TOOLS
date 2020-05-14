using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Module
{
    public static class Fixture
    {
        [CamundaWorkerTopic("UnifiedPortal.Module.Fixture.Before")]
        public class BeforeWorker : ModuleProcessingWorker
        {
            public BeforeWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore) : base(fileStore, moduleStore, portalSettingsStore)
            {
            }

            protected override void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client)
            {
                SendCommands(operation, logger, client, instructions.BeforeStep);
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Module.Fixture.After")]
        public class AfterWorker : ModuleProcessingWorker
        {
            public AfterWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore) : base(fileStore, moduleStore, portalSettingsStore)
            {
            }

            protected override void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client)
            {
                SendCommands(operation, logger, client, instructions.AfterStep);
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Module.Fixture.Error")]
        public class ErrorWorker : CamundaWorker
        {
            [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleId, Direction = CamundaVariableDirection.Input)]
            public int ModuleId { get; set; }

            [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleStatus, Direction = CamundaVariableDirection.Input)]
            public ModuleStatus ModuleStatus { get; set; }

            private readonly IModuleStore moduleStore;

            public ErrorWorker(IModuleStore moduleStore)
            {
                this.moduleStore = moduleStore;
            }

            public override void Execute(IOperation operation, IOperationLogger logger)
            {
                if (!moduleStore.IsModuleExists(operation, ModuleId).Result)
                {
                    throw CommonExceptions.ModuleWasNotFound(operation, ModuleId);
                }

                logger.Debug($"Module update didn't complete successfully, setting status to ${ModuleStatus:G}").Wait();

                moduleStore.ChangeStatus(operation, ModuleId, ModuleStatus).Wait();

                logger.Debug("Module status was updated").Wait();
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Module.Fixture.Status")]
        public class StatusWorker : CamundaWorker
        {
            [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleId, Direction = CamundaVariableDirection.Input)]
            public int ModuleId { get; set; }

            [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleStatus, Direction = CamundaVariableDirection.Both)]
            public ModuleStatus ModuleStatus { get; set; }

            private readonly IModuleStore moduleStore;

            public StatusWorker(IModuleStore moduleStore)
            {
                this.moduleStore = moduleStore;
            }

            public override void Execute(IOperation operation, IOperationLogger logger)
            {
                if (!moduleStore.IsModuleExists(operation, ModuleId).Result)
                {
                    throw CommonExceptions.ModuleWasNotFound(operation, ModuleId);
                }

                logger.Debug($"Setting module status to {ModuleStatus:G}").Wait();

                moduleStore.ChangeStatus(operation, ModuleId, ModuleStatus).Wait();

                logger.Debug("Module status was updated").Wait();
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Module.Fixture.Remove")]
        public class RemoveWorker : CamundaWorker
        {
            [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleId, Direction = CamundaVariableDirection.Input)]
            public int ModuleId { get; set; }

            private readonly IModuleStore moduleStore;

            public RemoveWorker(IModuleStore moduleStore)
            {
                this.moduleStore = moduleStore;
            }

            public override void Execute(IOperation operation, IOperationLogger logger)
            {
                if (!moduleStore.IsModuleExists(operation, ModuleId).Result)
                {
                    throw CommonExceptions.ModuleWasNotFound(operation, ModuleId);
                }

                logger.Debug("Removing module").Wait();

                moduleStore.DeleteModule(operation, ModuleId).Wait();

                logger.Debug("Module status was updated").Wait();
            }
        }
    }
}