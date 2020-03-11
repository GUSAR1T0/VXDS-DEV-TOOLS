using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Dashboard;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;
using VXDesign.Store.DevTools.Common.Storage.LogStorage;

namespace VXDesign.Store.DevTools.Common.Services
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