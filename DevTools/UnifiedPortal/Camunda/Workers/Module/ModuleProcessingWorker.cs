using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost.Extensions;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Core.Utils;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Module
{
    public abstract class ModuleProcessingWorker : CamundaWorkerWithErrorFallback
    {
        public override string ErrorCode => CamundaWorkerKey.ModuleProcessingError;

        protected virtual bool Regression => false;

        [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleId, Direction = CamundaVariableDirection.Input)]
        public int ModuleId { get; set; }

        [CamundaWorkerVariable(Name = CamundaWorkerKey.Action, Direction = CamundaVariableDirection.Input)]
        public ActionType Action { get; set; }

        private readonly IFileStore fileStore;
        private readonly IModuleStore moduleStore;
        private readonly IPortalSettingsStore portalSettingsStore;

        protected ModuleProcessingWorker(IFileStore fileStore, IModuleStore moduleStore, IPortalSettingsStore portalSettingsStore)
        {
            this.fileStore = fileStore;
            this.moduleStore = moduleStore;
            this.portalSettingsStore = portalSettingsStore;
        }

        protected abstract void ExecuteWithFileConfiguration(IOperation operation, IOperationLogger logger, OperatingSystemInstructions instructions, IRemoteHostClientService client);

        protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
        {
            if (!moduleStore.IsModuleExists(operation, ModuleId).Result)
            {
                throw CommonExceptions.ModuleWasNotFound(operation, ModuleId);
            }

            logger.Debug($"[Operation: {operation.ComplexOperationId}, module: {ModuleId}] Starting to download module file configuration").Wait();

            var module = moduleStore.GetModule(operation, ModuleId).Result;
            var fileId = (Regression ? Action switch
            {
                ActionType.Upgrade => module.PreviousConfiguration,
                ActionType.Downgrade => module.NextConfiguration,
                ActionType.NoChanges => module.CurrentConfiguration,
                _ => throw CommonExceptions.ModuleConfigurationWasNotFound(operation)
            } : module.CurrentConfiguration).FileId;
            var file = fileStore.Download(operation, fileId).Result;
            if (file == null)
            {
                throw CommonExceptions.FileWasNotFound(operation, fileId);
            }

            var configuration = ModuleConfigurationUtils.Parse(operation, file);
            if (!configuration.IsValid())
            {
                throw CommonExceptions.ModuleConfigurationIsInvalid(operation);
            }

            logger.Debug($"[Operation: {operation.ComplexOperationId}, module: {ModuleId}] Completed to download module file configuration").Wait();

            logger.Debug($"[Operation: {operation.ComplexOperationId}, module: {ModuleId}] Initializing client to host").Wait();

            var host = portalSettingsStore.GetHost(operation, module.HostId).Result;
            foreach (var credentials in host.CredentialsList)
            {
                if (credentials.CheckConnection(host.OperatingSystem, host.Domain, out var client).HasErrors) continue;

                logger.Debug($"[Operation: {operation.ComplexOperationId}, module: {ModuleId}] Credentials were found, starting to perform actions").Wait();

                using (client)
                {
                    ExecuteWithFileConfiguration(operation, logger, configuration.Instructions.FirstOrDefault(instructions => instructions.OperatingSystem == host.OperatingSystem), client);
                }

                logger.Debug($"[Operation: {operation.ComplexOperationId}, module: {ModuleId}] Finished to perform actions").Wait();

                return;
            }

            logger.Error($"[Operation: {operation.ComplexOperationId}, module: {ModuleId}] Failed to find credentials for connection to host").Wait();
            throw CommonExceptions.HostIsUnavailable(operation);
        }

        protected void SendCommands(IOperation operation, IOperationLogger logger, IRemoteHostClientService client, IEnumerable<ConfigurationCommand> commands)
        {
            foreach (var command in commands ?? new List<ConfigurationCommand>())
            {
                if (!string.IsNullOrWhiteSpace(command.Run))
                {
                    logger.Debug($"[Operation: {operation.ComplexOperationId}, module: {ModuleId}] Sending command: {command.Run}").Wait();

                    var result = client.SendWithErrorHandling(operation, command.Run);

                    logger.Debug($"[Operation: {operation.ComplexOperationId}, module: {ModuleId}] Result of command \"{command.Run}\" (exit status \"{result.ExitStatus}\"):\n{result.Output}").Wait();
                }
            }
        }
    }
}