using System;
using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Clients.Camunda;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Models.ProcessDefinition;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Core.Utils;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface IModuleService
    {
        #region Modules

        Task<ModulePagingResponse> GetItems(IOperation operation, ModulePagingRequest request);
        Task<ModuleEntity> GetModule(IOperation operation, int moduleId);
        Task UpdateModule(IOperation operation, int moduleId, int userId);
        Task LaunchModule(IOperation operation, int moduleId);
        Task StopModule(IOperation operation, int moduleId);
        Task UninstallModule(IOperation operation, int moduleId);

        #endregion

        #region Module Configurations

        Task<ModuleConfigurationFileUploadResult> ReadConfiguration(IOperation operation, UploadedFile file);
        Task<int> SubmitConfiguration(IOperation operation, ModuleConfigurationSubmitEntity entity);
        Task UpgradeModuleConfiguration(IOperation operation, int moduleId, int? userId);
        Task DowngradeModuleConfiguration(IOperation operation, int moduleId, int? userId);

        #endregion
    }

    public class ModuleService : IModuleService
    {
        private readonly IModuleStore moduleStore;
        private readonly IFileStore fileStore;
        private readonly IPortalSettingsStore portalSettingsStore;
        private readonly IUserDataStore userDataStore;
        private readonly ISyrinxCamundaClientService camundaClient;

        public ModuleService(IModuleStore moduleStore, IFileStore fileStore, IPortalSettingsStore portalSettingsStore, IUserDataStore userDataStore, ISyrinxCamundaClientService camundaClient)
        {
            this.moduleStore = moduleStore;
            this.fileStore = fileStore;
            this.portalSettingsStore = portalSettingsStore;
            this.userDataStore = userDataStore;
            this.camundaClient = camundaClient;
        }

        #region Modules

        public async Task<ModulePagingResponse> GetItems(IOperation operation, ModulePagingRequest request)
        {
            var (total, modules) = await moduleStore.GetModules(operation, request);
            return new ModulePagingResponse
            {
                Total = total,
                Items = modules
            };
        }

        public async Task<ModuleEntity> GetModule(IOperation operation, int moduleId)
        {
            if (!await moduleStore.IsModuleExists(operation, moduleId))
            {
                throw CommonExceptions.ModuleWasNotFound(operation, moduleId);
            }

            var module = await moduleStore.GetModule(operation, moduleId);
            var fileIds = module.Configurations.Select(item => item.FileId).ToList();
            var files = (await fileStore.Download(operation, fileIds)).ToList();
            foreach (var configuration in module.Configurations)
            {
                configuration.File = files.FirstOrDefault(file => file.Id == configuration.FileId);

                var fileConfiguration = ModuleConfigurationUtils.Parse(operation, configuration.File);
                if (!fileConfiguration.IsValid())
                {
                    throw CommonExceptions.ModuleConfigurationIsInvalid(operation);
                }

                configuration.OperatingSystems = fileConfiguration.Instructions.Where(item => item.OperatingSystem.HasValue).Select(item => item.OperatingSystem.Value);
            }

            return module;
        }

        public async Task UpdateModule(IOperation operation, int moduleId, int userId)
        {
            if (!await moduleStore.IsModuleExists(operation, moduleId))
            {
                throw CommonExceptions.ModuleWasNotFound(operation, moduleId);
            }

            await moduleStore.UpdateModule(operation, moduleId, userId);
        }

        public async Task LaunchModule(IOperation operation, int moduleId)
        {
            if (!await moduleStore.IsModuleExists(operation, moduleId))
            {
                throw CommonExceptions.ModuleWasNotFound(operation, moduleId);
            }

            if (!(await moduleStore.HasStatuses(operation, moduleId, ModuleStatus.Stopped)).Any())
            {
                throw CommonExceptions.FailedToRunModule(operation);
            }

            await moduleStore.ChangeStatus(operation, moduleId, ModuleStatus.UpdatedToRun);
            await new ProcessDefinition.StartProcessInstanceByKeyRequest(CamundaWorkerKey.ModuleLaunchProcess)
            {
                BusinessKey = moduleId.ToString(),
                Variables = new CamundaVariables { { CamundaWorkerKey.ModuleId, moduleId } }
            }.SendRequest(operation, camundaClient, true);
        }

        public async Task StopModule(IOperation operation, int moduleId)
        {
            if (!await moduleStore.IsModuleExists(operation, moduleId))
            {
                throw CommonExceptions.ModuleWasNotFound(operation, moduleId);
            }

            if (!(await moduleStore.HasStatuses(operation, moduleId, ModuleStatus.Run)).Any())
            {
                throw CommonExceptions.FailedToStopModule(operation);
            }

            await moduleStore.ChangeStatus(operation, moduleId, ModuleStatus.UpdatedToStop);
            await new ProcessDefinition.StartProcessInstanceByKeyRequest(CamundaWorkerKey.ModuleStopProcess)
            {
                BusinessKey = moduleId.ToString(),
                Variables = new CamundaVariables { { CamundaWorkerKey.ModuleId, moduleId } }
            }.SendRequest(operation, camundaClient, true);
        }

        public async Task UninstallModule(IOperation operation, int moduleId)
        {
            if (!await moduleStore.IsModuleExists(operation, moduleId))
            {
                throw CommonExceptions.ModuleWasNotFound(operation, moduleId);
            }

            var statuses = (await moduleStore.HasStatuses(operation, moduleId, ModuleStatus.Run, ModuleStatus.Stopped)).ToList();
            if (!statuses.Any())
            {
                throw CommonExceptions.FailedToUninstallModule(operation);
            }

            await moduleStore.ChangeStatus(operation, moduleId, ModuleStatus.UpdatedToUninstall);
            await new ProcessDefinition.StartProcessInstanceByKeyRequest(CamundaWorkerKey.ModuleUninstallationProcess)
            {
                BusinessKey = moduleId.ToString(),
                Variables = new CamundaVariables
                {
                    { CamundaWorkerKey.ModuleId, moduleId },
                    { CamundaWorkerKey.ComponentsStopRequired, statuses.Contains(ModuleStatus.Run) }
                }
            }.SendRequest(operation, camundaClient, true);
        }

        #endregion

        #region Module Configurations

        public async Task<ModuleConfigurationFileUploadResult> ReadConfiguration(IOperation operation, UploadedFile file)
        {
            var configuration = ModuleConfigurationUtils.Parse(operation, file);
            if (!configuration.IsValid())
            {
                throw CommonExceptions.ModuleConfigurationIsInvalid(operation);
            }

            var module = await moduleStore.GetModuleByAlias(operation, configuration.Alias);
            var result = new ModuleConfigurationFileUploadResult
            {
                FileId = await fileStore.Find(operation, file) ?? await fileStore.Upload(operation, file),
                Alias = configuration.Alias.ToUpper(),
                NewName = configuration.Name,
                NewVersion = configuration.Version,
                OperatingSystems = configuration.Instructions.Where(item => item.OperatingSystem.HasValue).Select(item => item.OperatingSystem.Value)
            };

            if (module == null)
            {
                // Module is new for the system
                result.Verdict = ModuleConfigurationVerdict.NewModule;
                return result;
            }

            // Module had already installed but update may be required
            result.ModuleId = module.Id;
            result.OldName = module.Name;
            result.OldVersion = module.Version;
            result.UserId = module.UserId;
            result.FirstName = module.FirstName;
            result.LastName = module.LastName;
            result.HostId = module.HostId;
            result.HostName = module.HostName;
            result.HostDomain = module.HostDomain;
            result.HostOperatingSystem = module.HostOperatingSystem;

            if (!result.OperatingSystems.Contains(module.HostOperatingSystem))
            {
                // Configuration doesn't have instructions for OS which used on current host
                result.Verdict = ModuleConfigurationVerdict.BrokenChanges;
                return result;
            }

            if (module.Status == ModuleStatus.Run || module.Status == ModuleStatus.Stopped)
            {
                // Module in stable mode: run or stopped
                result.Verdict = GetVerdict(operation, module.Version, configuration.Version);
                return result;
            }

            result.Verdict = ModuleConfigurationVerdict.Updating;
            return result;
        }

        public async Task<int> SubmitConfiguration(IOperation operation, ModuleConfigurationSubmitEntity entity)
        {
            var file = await fileStore.Download(operation, entity.FileId);
            if (file == null)
            {
                throw CommonExceptions.FileWasNotFound(operation, entity.FileId);
            }

            var configuration = ModuleConfigurationUtils.Parse(operation, file);
            if (!configuration.IsValid())
            {
                throw CommonExceptions.ModuleConfigurationIsInvalid(operation);
            }

            var module = await moduleStore.GetModuleByAlias(operation, configuration.Alias);
            if (module == null)
            {
                if (entity.ModuleId != null)
                {
                    throw CommonExceptions.FailedToDefineModuleForSubmission(operation);
                }

                if (!await userDataStore.IsUserExist(operation, entity.UserId))
                {
                    throw CommonExceptions.UserWasNotFound(operation, entity.UserId);
                }

                if (!await portalSettingsStore.IsHostExist(operation, entity.HostId))
                {
                    throw CommonExceptions.HostWasNotFound(operation, entity.HostId);
                }

                // Module is new for the system
                var moduleIdForCreate = await moduleStore.CreateModule(operation, entity.UserId, entity.HostId, entity.FileId, configuration);
                await new ProcessDefinition.StartProcessInstanceByKeyRequest(CamundaWorkerKey.ModuleInstallationProcess)
                {
                    BusinessKey = moduleIdForCreate.ToString(),
                    Variables = new CamundaVariables { { CamundaWorkerKey.ModuleId, moduleIdForCreate } }
                }.SendRequest(operation, camundaClient, true);
                return moduleIdForCreate;
            }

            var operatingSystems = configuration.Instructions.Where(item => item.OperatingSystem.HasValue).Select(item => item.OperatingSystem.Value);
            if (!operatingSystems.Contains(module.HostOperatingSystem))
            {
                // Configuration doesn't have instructions for OS which used on current host
                throw CommonExceptions.FailedToSubmitModuleConfiguration(operation, ModuleConfigurationVerdict.BrokenChanges);
            }

            if (module.Status == ModuleStatus.Run || module.Status == ModuleStatus.Stopped)
            {
                if (!await userDataStore.IsUserExist(operation, entity.UserId))
                {
                    throw CommonExceptions.UserWasNotFound(operation, entity.UserId);
                }

                // Module in stable mode: run or stopped
                var moduleIdForUpgrade = ValidateToUpgradeModule(operation, entity.ModuleId, module, configuration);
                await moduleStore.UpgradeModule(operation, moduleIdForUpgrade, entity.UserId, entity.FileId, configuration);
                await new ProcessDefinition.StartProcessInstanceByKeyRequest(CamundaWorkerKey.ModuleUpgradeProcess)
                {
                    BusinessKey = moduleIdForUpgrade.ToString(),
                    Variables = new CamundaVariables
                    {
                        { CamundaWorkerKey.ModuleId, moduleIdForUpgrade },
                        { CamundaWorkerKey.ComponentsStopRequired, module.Status == ModuleStatus.Run }
                    }
                }.SendRequest(operation, camundaClient, true);
                return moduleIdForUpgrade;
            }

            throw CommonExceptions.FailedToSubmitModuleConfiguration(operation, ModuleConfigurationVerdict.Updating);
        }

        private static ModuleConfigurationVerdict GetVerdict(IOperation operation, string oldVersion, string newVersion)
        {
            var savedConfigVersionSegments = oldVersion.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            var newConfigVersionSegments = newVersion.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            if (savedConfigVersionSegments.Length != newConfigVersionSegments.Length)
            {
                throw CommonExceptions.ModuleConfigurationsHaveDifferentLength(operation);
            }

            if (savedConfigVersionSegments.Where((segment, index) => segment == newConfigVersionSegments[index]).Count() == newConfigVersionSegments.Length)
            {
                return ModuleConfigurationVerdict.Installed;
            }

            if (!savedConfigVersionSegments.Where((segment, index) => segment > newConfigVersionSegments[index]).Any())
            {
                return ModuleConfigurationVerdict.Upgrade;
            }

            return ModuleConfigurationVerdict.Downgrade;
        }

        private static int ValidateToUpgradeModule(IOperation operation, int? moduleId, ModuleInfoEntity module, ModuleConfigurationFile configuration)
        {
            return GetVerdict(operation, module.Version, configuration.Version) switch
            {
                ModuleConfigurationVerdict.Upgrade => moduleId ?? throw CommonExceptions.FailedToDefineModuleForUpgrade(operation),
                ModuleConfigurationVerdict.Installed => throw CommonExceptions.FailedToSubmitModuleConfiguration(operation, ModuleConfigurationVerdict.Installed),
                ModuleConfigurationVerdict.Downgrade => throw CommonExceptions.FailedToSubmitModuleConfiguration(operation, ModuleConfigurationVerdict.Downgrade),
                ModuleConfigurationVerdict.Updating => throw new ArgumentOutOfRangeException(),
                ModuleConfigurationVerdict.BrokenChanges => throw new ArgumentOutOfRangeException(),
                ModuleConfigurationVerdict.NewModule => throw new ArgumentOutOfRangeException(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public async Task UpgradeModuleConfiguration(IOperation operation, int moduleId, int? userId)
        {
            var module = await moduleStore.GetModule(operation, moduleId);
            if (module == null)
            {
                throw CommonExceptions.ModuleWasNotFound(operation, moduleId);
            }

            if (module.Status != ModuleStatus.Run && module.Status != ModuleStatus.Stopped)
            {
                throw CommonExceptions.FailedToUpgradeModuleConfigurationDueToModuleStatus(operation);
            }

            if (module.NextConfiguration == null)
            {
                throw CommonExceptions.NoModuleConfigurationForUpgrade(operation);
            }

            var file = await fileStore.Download(operation, module.NextConfiguration.FileId);
            if (file == null)
            {
                throw CommonExceptions.FileWasNotFound(operation, module.NextConfiguration.FileId);
            }

            var configuration = ModuleConfigurationUtils.Parse(operation, file);
            if (!configuration.IsValid())
            {
                throw CommonExceptions.ModuleConfigurationIsInvalid(operation);
            }

            var operatingSystems = configuration.Instructions.Where(item => item.OperatingSystem.HasValue).Select(item => item.OperatingSystem.Value);
            if (!operatingSystems.Contains(module.HostOperatingSystem))
            {
                // Configuration doesn't have instructions for OS which used on current host
                throw CommonExceptions.FailedToUpgradeModuleConfigurationDueToOperatingSystemConflict(operation);
            }

            if (GetVerdict(operation, module.Version, module.NextConfiguration.Version) != ModuleConfigurationVerdict.Upgrade)
            {
                throw CommonExceptions.FailedToUpgradeModuleConfigurationDueToVerdict(operation);
            }

            await moduleStore.UpgradeModule(operation, module.Id, userId ?? module.UserId, module.NextConfiguration.Id);
            await new ProcessDefinition.StartProcessInstanceByKeyRequest(CamundaWorkerKey.ModuleUpgradeProcess)
            {
                BusinessKey = moduleId.ToString(),
                Variables = new CamundaVariables 
                { 
                    { CamundaWorkerKey.ModuleId, moduleId },
                    { CamundaWorkerKey.ComponentsStopRequired, module.Status == ModuleStatus.Run }
                }
            }.SendRequest(operation, camundaClient, true);
        }

        public async Task DowngradeModuleConfiguration(IOperation operation, int moduleId, int? userId)
        {
            var module = await moduleStore.GetModule(operation, moduleId);
            if (module == null)
            {
                throw CommonExceptions.ModuleWasNotFound(operation, moduleId);
            }

            if (module.Status != ModuleStatus.Run && module.Status != ModuleStatus.Stopped)
            {
                throw CommonExceptions.FailedToDowngradeModuleConfigurationDueToModuleStatus(operation);
            }

            if (module.PreviousConfiguration == null)
            {
                throw CommonExceptions.NoModuleConfigurationForDowngrade(operation);
            }

            var file = await fileStore.Download(operation, module.PreviousConfiguration.FileId);
            if (file == null)
            {
                throw CommonExceptions.FileWasNotFound(operation, module.PreviousConfiguration.FileId);
            }

            var configuration = ModuleConfigurationUtils.Parse(operation, file);
            if (!configuration.IsValid())
            {
                throw CommonExceptions.ModuleConfigurationIsInvalid(operation);
            }

            var operatingSystems = configuration.Instructions.Where(item => item.OperatingSystem.HasValue).Select(item => item.OperatingSystem.Value);
            if (!operatingSystems.Contains(module.HostOperatingSystem))
            {
                // Configuration doesn't have instructions for OS which used on current host
                throw CommonExceptions.FailedToDowngradeModuleConfigurationDueToOperatingSystemConflict(operation);
            }

            if (GetVerdict(operation, module.Version, module.PreviousConfiguration.Version) != ModuleConfigurationVerdict.Downgrade)
            {
                throw CommonExceptions.FailedToDowngradeModuleConfigurationDueToVerdict(operation);
            }

            await moduleStore.DowngradeModule(operation, module.Id, userId ?? module.UserId, module.PreviousConfiguration.Id);
            await new ProcessDefinition.StartProcessInstanceByKeyRequest(CamundaWorkerKey.ModuleRollbackProcess)
            {
                BusinessKey = moduleId.ToString(),
                Variables = new CamundaVariables
                {
                    { CamundaWorkerKey.ModuleId, moduleId },
                    { CamundaWorkerKey.ComponentsStopRequired, module.Status == ModuleStatus.Run }
                }
            }.SendRequest(operation, camundaClient, true);
        }

        #endregion
    }
}