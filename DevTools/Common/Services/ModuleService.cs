using System;
using System.Linq;
using System.Threading.Tasks;
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
        Task<ModulePagingResponse> GetItems(IOperation operation, ModulePagingRequest request);
        Task<ModuleConfigurationFileUploadResult> ReadConfiguration(IOperation operation, UploadedFile file);
        Task<int> SubmitConfiguration(IOperation operation, ModuleConfigurationSubmitEntity entity);
    }

    public class ModuleService : IModuleService
    {
        private readonly IModuleStore moduleStore;
        private readonly IFileStore fileStore;
        private readonly IPortalSettingsStore portalSettingsStore;
        private readonly IUserDataStore userDataStore;

        public ModuleService(IModuleStore moduleStore, IFileStore fileStore, IPortalSettingsStore portalSettingsStore, IUserDataStore userDataStore)
        {
            this.moduleStore = moduleStore;
            this.fileStore = fileStore;
            this.portalSettingsStore = portalSettingsStore;
            this.userDataStore = userDataStore;
        }

        public async Task<ModulePagingResponse> GetItems(IOperation operation, ModulePagingRequest request)
        {
            var (total, modules) = await moduleStore.GetModules(operation, request);
            return new ModulePagingResponse
            {
                Total = total,
                Items = modules
            };
        }

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
                // TODO: Camunda trigger
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
                // TODO: Camunda trigger
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
    }
}