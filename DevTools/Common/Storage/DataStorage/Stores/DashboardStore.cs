using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Dashboard;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IDashboardStore
    {
        Task<DashboardEntity> GetDashboardDataFromDatabase(IOperation operation);
    }

    public class DashboardStore : BaseDataStore, IDashboardStore
    {
        public async Task<DashboardEntity> GetDashboardDataFromDatabase(IOperation operation)
        {
            return await operation.Connection.QueryFirstAsync<DashboardEntity>(@"
                DECLARE @UsersCount INT, @RolesCount INT, @OperationsCount BIGINT;

                SELECT @UsersCount = COUNT(*)
                FROM [authentication].[User];

                SELECT @RolesCount = COUNT(*)
                FROM [authentication].[UserRole];

                SELECT @OperationsCount = COUNT_BIG(1)
                FROM [base].[Operation];

                SELECT
                    @UsersCount AS [UsersCount],
                    @RolesCount AS [RolesCount],
                    @OperationsCount AS [OperationsCount];
            ");
        }
    }
}