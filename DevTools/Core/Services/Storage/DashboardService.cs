using System.Threading.Tasks;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Entities.Storage;
using VXDesign.Store.DevTools.Core.Storage.DataStores;

namespace VXDesign.Store.DevTools.Core.Services.Storage
{
    public interface IDashboardService
    {
        Task<DashboardEntity> GetDashboardData(IOperation operation);
    }

    public class DashboardService : IDashboardService
    {
        private readonly IDashboardStore dashboardStore;

        public DashboardService(IDashboardStore dashboardStore)
        {
            this.dashboardStore = dashboardStore;
        }

        public async Task<DashboardEntity> GetDashboardData(IOperation operation)
        {
            var entity = await dashboardStore.GetDashboardDataFromDatabase(operation);
            entity.LogsCount = await operation.Logger<DashboardStore>().CountOfAllCollections();
            return entity;
        }
    }
}