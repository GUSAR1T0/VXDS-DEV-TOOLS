using System.Threading.Tasks;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Entities.Storage;

namespace VXDesign.Store.DevTools.Core.Storage.DataStores
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
                FROM [authentication].[User]

                SELECT @RolesCount = COUNT(*)
                FROM [authentication].[UserRole]

                SELECT
                    @UsersCount AS [UsersCount],
                    @RolesCount AS [RolesCount]
            ");
        }
    }
}