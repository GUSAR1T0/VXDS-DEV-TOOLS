using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Storage;

namespace VXDesign.Store.DevTools.Common.Storage.DataStores
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
                DECLARE @UsersCount INT, @RolesCount INT

                SELECT @UsersCount = COUNT(*)
                FROM [authorization].[User]

                SELECT @RolesCount = COUNT(*)
                FROM [authorization].[UserRole]

                SELECT
                    @UsersCount AS [UsersCount],
                    @RolesCount AS [RolesCount]
            ");
        }
    }
}