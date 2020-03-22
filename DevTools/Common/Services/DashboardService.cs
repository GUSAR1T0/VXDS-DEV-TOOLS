using System;
using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Dashboard;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface IDashboardService
    {
        Task<UsersDataEntity> GetUsersData(IOperation operation);
        Task<UserRolesDataEntity> GetUserRolesData(IOperation operation);
        Task<ProjectsDataEntity> GetProjectsData(IOperation operation);
        Task<SystemStatisticsDataEntity> GetSystemStatisticsData(IOperation operation);
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

        public async Task<UsersDataEntity> GetUsersData(IOperation operation) => await dashboardStore.GetUsersData(operation);

        public async Task<UserRolesDataEntity> GetUserRolesData(IOperation operation)
        {
            var (userRoles, total) = await dashboardStore.GetUserRolesData(operation);
            return new UserRolesDataEntity
            {
                UserRoles = userRoles,
                Total = total
            };
        }

        public async Task<ProjectsDataEntity> GetProjectsData(IOperation operation) => await dashboardStore.GetProjectsData(operation);

        public async Task<SystemStatisticsDataEntity> GetSystemStatisticsData(IOperation operation)
        {
            var today = DateTime.Now.ToUniversalTime().Date;
            var sevenDaysAgo = today.AddDays(-6).Date;
            var (operations, operationsTotal) = await dashboardStore.GetOperationsData(operation, sevenDaysAgo, today);
            var (logs, logsTotal) = await loggerStore.GetLogsData(sevenDaysAgo, today);
            return new SystemStatisticsDataEntity
            {
                SevenDaysAgo = sevenDaysAgo,
                Today = today,
                Operations = operations,
                OperationsTotal = operationsTotal,
                Logs = logs.Select(entity => new SystemStatisticsDataByDateEntity
                {
                    Date = entity.Date,
                    Count = entity.Count
                }),
                LogsTotal = logsTotal
            };
        }
    }
}