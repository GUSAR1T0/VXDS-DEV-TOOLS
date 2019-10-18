using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Storage;

namespace VXDesign.Store.DevTools.Common.Storage.DataStores
{
    public interface IUserRoleStore
    {
        Task<UserRoleEntity> GetUserRoleById(IOperation operation, int id);
        Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation);
        Task AddUserRole(IOperation operation, UserRoleEntity entity);
        Task UpdateUserRole(IOperation operation, UserRoleEntity entity);
        Task DeleteUserRoleById(IOperation operation, int id);
    }

    public class UserRoleStore : BaseDataStore, IUserRoleStore
    {
        public async Task<UserRoleEntity> GetUserRoleById(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<UserRoleEntity>(new { Id = id }, @"
                SELECT
                    [Id],
                    [Name]
                FROM [authorization].[UserRole]
                WHERE [Id] = @Id
            ");
        }

        public async Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation)
        {
            return await operation.Connection.QueryAsync<UserRoleEntity>(@"
                SELECT
                    [Id],
                    [Name]
                FROM [authorization].[UserRole]
            ");
        }

        public async Task AddUserRole(IOperation operation, UserRoleEntity entity)
        {
            await operation.Connection.ExecuteAsync(new { entity.Name }, @"
                INSERT INTO [authorization].[UserRole] ([Name])
                VALUES (@Name)
            ");
        }

        public async Task UpdateUserRole(IOperation operation, UserRoleEntity entity)
        {
            await operation.Connection.ExecuteAsync(new
            {
                entity.Id,
                entity.Name
            }, @"
                UPDATE [authorization].[UserRole]
                SET [Name] = @Name
                WHERE [Id] = @Id
            ");
        }

        public async Task DeleteUserRoleById(IOperation operation, int id)
        {
            await operation.Connection.ExecuteAsync(new { Id = id }, @"
                DELETE FROM [authorization].[UserRole]
                WHERE [Id] = @Id
            ");
        }
    }
}