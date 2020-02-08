using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Entities.Storage;

namespace VXDesign.Store.DevTools.Core.Storage.DataStores
{
    public interface IUserRoleStore
    {
        Task<UserRoleEntity> GetUserRoleById(IOperation operation, int id);
        Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation, bool isFullInfoNeeded);
        Task AddUserRole(IOperation operation, UserRoleEntity entity);
        Task UpdateUserRole(IOperation operation, UserRoleEntity entity);
        Task DeleteUserRoleById(IOperation operation, int id);
        Task<bool> IsUserRoleExist(IOperation operation, int id);
        Task<bool> IsUserRoleExist(IOperation operation, string name, int? id = null);
    }

    public class UserRoleStore : BaseDataStore, IUserRoleStore
    {
        public async Task<UserRoleEntity> GetUserRoleById(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<UserRoleEntity>(new { Id = id }, @"
                SELECT
                    [Id],
                    [Name],
                    [PortalPermissions]
                FROM [authentication].[UserRole]
                WHERE [Id] = @Id;
            ");
        }

        public async Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation, bool isFullInfoNeeded)
        {
            var fieldsOfFullInfoQuery = isFullInfoNeeded
                ? @",
                    [PortalPermissions]"
                : "";
            return await operation.Connection.QueryAsync<UserRoleEntity>($@"
                SELECT
                    [Id],
                    [Name]
                    {fieldsOfFullInfoQuery}
                FROM [authentication].[UserRole]
            ");
        }

        public async Task AddUserRole(IOperation operation, UserRoleEntity entity)
        {
            await operation.Connection.ExecuteAsync(new
            {
                entity.Name,
                entity.PortalPermissions
            }, @"
                INSERT INTO [authentication].[UserRole] (
                    [Name],
                    [PortalPermissions]
                )
                VALUES (
                    @Name,
                    @PortalPermissions
                )
            ");
        }

        public async Task UpdateUserRole(IOperation operation, UserRoleEntity entity)
        {
            await operation.Connection.ExecuteAsync(new
            {
                entity.Id,
                entity.Name,
                entity.PortalPermissions
            }, @"
                UPDATE [authentication].[UserRole]
                SET
                    [Name] = @Name,
                    [PortalPermissions] = @PortalPermissions
                WHERE [Id] = @Id
            ");
        }

        public async Task DeleteUserRoleById(IOperation operation, int id)
        {
            await operation.Connection.ExecuteAsync(new { Id = id }, @"
                DELETE FROM [authentication].[UserRole]
                WHERE [Id] = @Id
            ");
        }

        public async Task<bool> IsUserRoleExist(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { Id = id }, @"
                SELECT TOP (1) 1
                FROM [authentication].[UserRole]
                WHERE [Id] = @Id;
            ");
        }

        public async Task<bool> IsUserRoleExist(IOperation operation, string name, int? id = null)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new
            {
                Name = name,
                Id = id
            }, @"
                SELECT TOP (1) 1
                FROM [authentication].[UserRole]
                WHERE [Name] = @Name AND (@Id IS NULL OR [Id] <> @Id);
            ");
        }
    }
}