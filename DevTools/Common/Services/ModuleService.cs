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
    }

    public class ModuleService : IModuleService
    {
        private readonly IModuleStore moduleStore;
        private readonly IFileStore fileStore;

        public ModuleService(IModuleStore moduleStore, IFileStore fileStore)
        {
            this.moduleStore = moduleStore;
            this.fileStore = fileStore;
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
                NewVersion = configuration.Version
            };

            if (module == null)
            {
                // Module is new for the system
                result.Verdict = ModuleConfigurationVerdict.NewModule;
                return result;
            }

            // Module had already installed but update may be required
            result.Id = module.Id;
            result.OldName = module.Name;
            result.OldVersion = module.Version;
            result.Verdict = GetVerdict(operation, module.Version, configuration.Version);
            return result;
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
    }
}