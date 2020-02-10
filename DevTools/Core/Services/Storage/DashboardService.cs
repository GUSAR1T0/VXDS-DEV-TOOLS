using System.Threading.Tasks;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Entities.Storage.Dashboard;
using VXDesign.Store.DevTools.Core.Storage.DataStores;
using VXDesign.Store.DevTools.Core.Storage.LogStores;

namespace VXDesign.Store.DevTools.Core.Services.Storage
{
    public interface IDashboardService
    {
        Task<DashboardEntity> GetDashboardData(IOperation operation);
    }

    public class DashboardService : IDashboardService
    {
        private readonly IDashboardStore dashboardStore;
        private readonly ILoggerStore loggerStore;

        public DashboardService(IDashboardStore dashboardStore, ILoggerStore loggerStore)
        {
            this.dashboardStore = dashboardStore;
            this.loggerStore = loggerStore;
        }

        public async Task<DashboardEntity> GetDashboardData(IOperation operation)
        {
            var entity = await dashboardStore.GetDashboardDataFromDatabase(operation);
            entity.LogsCount = await loggerStore.GetCount();
            return entity;
        }
    }
}