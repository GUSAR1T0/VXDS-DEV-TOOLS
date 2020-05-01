using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface IModuleService
    {
        Task<ModulePagingResponse> GetItems(IOperation operation, ModulePagingRequest request);
    }

    public class ModuleService : IModuleService
    {
        private readonly IModuleStore moduleStore;

        public ModuleService(IModuleStore moduleStore)
        {
            this.moduleStore = moduleStore;
        }

        public async Task<ModulePagingResponse> GetItems(IOperation operation, ModulePagingRequest request)
        {
            var (total, modules) = await moduleStore.Get(operation, request);
            return new ModulePagingResponse
            {
                Total = total,
                Items = modules
            };
        }
    }
}