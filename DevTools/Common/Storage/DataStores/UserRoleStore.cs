using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Storage;
using VXDesign.Store.DevTools.Common.Enums.Operations;

namespace VXDesign.Store.DevTools.Common.Storage.DataStores
{
    public interface IUserRoleStore
    {
        Task<UserRoleEntity> GetUserRoleById(IOperation operation, int id);
        Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation, bool isFullInfoNeeded);
        Task AddUserRole(IOperation operation, UserRoleEntity entity);
        Task UpdateUserRole(IOperation operation, UserRoleEntity entity);
        Task DeleteUserRoleById(IOperation operation, int id);
        Task<bool> IsUserRoleExist(IOperation operation, int id);
    }

    public class UserRoleStore : BaseDataStore, IUserRoleStore
    {
        public async Task<UserRoleEntity> GetUserRoleById(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<UserRoleEntity>(new { Id = id }, @"
                SELECT
                    [Id],
                    [Name],
                    [UserPermissions]
                FROM [authorization].[UserRole]
                WHERE [Id] = @Id;
            ");
        }

        public async Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation, bool isFullInfoNeeded)
        {
            var fieldsOfFullInfoQuery = isFullInfoNeeded
                ? @",
                    [UserPermissions]"
                : "";
            return await operation.Connection.QueryAsync<UserRoleEntity>($@"
                SELECT
                    [Id],
                    [Name]
                    {fieldsOfFullInfoQuery}
                FROM [authorization].[UserRole]
            ");
        }

        public async Task AddUserRole(IOperation operation, UserRoleEntity entity)
        {
            await operation.Connection.ExecuteAsync(new
            {
                entity.Name,
                entity.UserPermissions
            }, @"
                INSERT INTO [authorization].[UserRole] (
                    [Name],
                    [UserPermissions]
                )
                VALUES (
                    @Name,
                    @UserPermissions
                )
            ");
        }

        public async Task UpdateUserRole(IOperation operation, UserRoleEntity entity)
        {
            await operation.Connection.ExecuteAsync(new
            {
                entity.Id,
                entity.Name,
                entity.UserPermissions
            }, @"
                UPDATE [authorization].[UserRole]
                SET
                    [Name] = @Name,
                    [UserPermissions] = @UserPermissions
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

        public async Task<bool> IsUserRoleExist(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { Id = id }, @"
                SELECT TOP (1) 1
                FROM [authorization].[UserRole]
                WHERE [Id] = @Id;
            ");
        }
    }
}