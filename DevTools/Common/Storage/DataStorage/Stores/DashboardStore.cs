using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Dashboard;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IDashboardStore
    {
        Task<UsersDataEntity> GetUsersData(IOperation operation);
        Task<(IEnumerable<UserRoleDataEntity> userRoles, int total)> GetUserRolesData(IOperation operation);
        Task<ProjectsDataEntity> GetProjectsData(IOperation operation);
        Task<(IEnumerable<SystemStatisticsDataByDateEntity> operations, long total)> GetOperationsData(IOperation operation, DateTime sevenDaysAgo, DateTime today);
    }

    public class DashboardStore : BaseDataStore, IDashboardStore
    {
        public async Task<UsersDataEntity> GetUsersData(IOperation operation)
        {
            return await operation.Connection.QueryFirstAsync<UsersDataEntity>(@"
                SELECT
                    COUNT(IIF([IsActivated] = 1, 1, NULL)) AS [ActivatedCount],
                    COUNT(IIF([IsActivated] = 0, 1, NULL)) AS [DeactivatedCount]
                FROM [authentication].[User];
            ");
        }

        public async Task<(IEnumerable<UserRoleDataEntity> userRoles, int total)> GetUserRolesData(IOperation operation)
        {
            var reader = await operation.Connection.QueryMultipleAsync(@"
                SELECT TOP 3 aur.[Name], COUNT(au.[Id]) AS [Count]
                FROM [authentication].[UserRole] aur
                INNER JOIN [authentication].[User] au ON aur.[Id] = au.[UserRoleId]
                GROUP BY aur.[Name]
                ORDER BY COUNT(au.[Id]) DESC, aur.[Name];

                SELECT COUNT(*) FROM [authentication].[UserRole];
            ");
            var userRoles = await reader.ReadAsync<UserRoleDataEntity>();
            var total = await reader.ReadSingleOrDefaultAsync<int>();
            return (userRoles, total);
        }

        public async Task<ProjectsDataEntity> GetProjectsData(IOperation operation)
        {
            return await operation.Connection.QueryFirstAsync<ProjectsDataEntity>(@"
                SELECT
                    COUNT(IIF([IsActive] = 1, 1, NULL)) AS [ActiveCount],
                    COUNT(IIF([IsActive] = 0, 1, NULL)) AS [InactiveCount]
                FROM [portal].[Project];
            ");
        }

        public async Task<(IEnumerable<SystemStatisticsDataByDateEntity> operations, long total)> GetOperationsData(IOperation operation, DateTime sevenDaysAgo, DateTime today)
        {
            var reader = await operation.Connection.QueryMultipleAsync(new
            {
                SevenDaysAgo = sevenDaysAgo,
                Today = today
            },@"
                SELECT operations.[Date], COUNT_BIG(*) AS [Count]
                FROM (
                    SELECT CONVERT(DATE, [StartTime]) AS [Date]
                    FROM [base].[Operation]
                ) operations
                WHERE operations.[Date] BETWEEN @SevenDaysAgo AND @Today
                GROUP BY operations.[Date]
                ORDER BY operations.[Date];

                SELECT COUNT_BIG(*) FROM [base].[Operation];
            ");
            var operations = await reader.ReadAsync<SystemStatisticsDataByDateEntity>();
            var total = await reader.ReadSingleOrDefaultAsync<long>();
            return (operations, total);
        }
    }
}