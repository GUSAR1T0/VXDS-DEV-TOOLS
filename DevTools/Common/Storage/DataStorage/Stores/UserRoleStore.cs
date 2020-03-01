using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Permission;
using VXDesign.Store.DevTools.Common.Core.Entities.User;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IUserRoleStore
    {
        Task<UserRoleWithPermissionsEntity> GetUserRoleById(IOperation operation, int id);
        Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation);
        Task<IEnumerable<UserRoleWithPermissionsEntity>> GetUserRolesWithPermissions(IOperation operation);
        Task<IEnumerable<UserRoleEntity>> SearchUserRolesByPattern(IOperation operation, string pattern);
        Task AddUserRole(IOperation operation, UserRoleWithPermissionsEntity entity);
        Task UpdateUserRole(IOperation operation, UserRoleWithPermissionsEntity entity);
        Task DeleteUserRoleById(IOperation operation, int id);
        Task<bool> IsUserRoleExist(IOperation operation, int id);
        Task<bool> IsUserRoleExist(IOperation operation, string name, int? id = null);
        Task<IEnumerable<PermissionGroupEntity>> GetUserRolePermissions(IOperation operation);
    }

    public class UserRoleStore : BaseDataStore, IUserRoleStore
    {
        private const string BaseSelect = @"
            SELECT
                [Id],
                [Name]
            FROM [authentication].[UserRole]
        ";

        public async Task<UserRoleWithPermissionsEntity> GetUserRoleById(IOperation operation, int id)
        {
            var reader = await operation.Connection.QueryMultipleAsync(new { Id = id }, $@"
                {BaseSelect} WHERE [Id] = @Id;

                SELECT
                    [PermissionGroupId],
                    [Permissions]
                FROM [authentication].[UserRolePermission]
                WHERE [UserRoleId] = @Id;
            ");
            var userRole = await reader.ReadSingleOrDefaultAsync<UserRoleEntity>();
            return userRole != null
                ? new UserRoleWithPermissionsEntity
                {
                    Id = userRole.Id,
                    Name = userRole.Name,
                    Permissions = await reader.ReadAsync<UserRolePermissionEntity>()
                }
                : null;
        }

        public async Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation)
        {
            return await operation.Connection.QueryAsync<UserRoleEntity>(BaseSelect);
        }

        public async Task<IEnumerable<UserRoleWithPermissionsEntity>> GetUserRolesWithPermissions(IOperation operation)
        {
            var reader = await operation.Connection.QueryMultipleAsync($@"
                {BaseSelect};

                SELECT
                    [UserRoleId],
                    [PermissionGroupId],
                    [Permissions]
                FROM [authentication].[UserRolePermission];
            ");
            var userRoles = await reader.ReadAsync<UserRoleEntity>();
            var userRolePermissions = await reader.ReadAsync<UserRolePermissionExtendedEntity>();
            return userRoles.Select(userRole => new UserRoleWithPermissionsEntity
            {
                Id = userRole.Id,
                Name = userRole.Name,
                Permissions = userRolePermissions.Where(item => item.UserRoleId == userRole.Id)
            });
        }

        public async Task<IEnumerable<UserRoleEntity>> SearchUserRolesByPattern(IOperation operation, string pattern)
        {
            return await operation.Connection.QueryAsync<UserRoleEntity>(new { Pattern = $"%{pattern}%" }, $@"{BaseSelect} WHERE [Name] LIKE @Pattern;");
        }

        public async Task AddUserRole(IOperation operation, UserRoleWithPermissionsEntity entity)
        {
            await operation.Connection.ExecuteAsync(new
            {
                entity.Name,
                Permissions = new UserRolePermissionEntities(entity.Permissions).ToTable()
            }, @"
                DECLARE @Ids TABLE ([Id] INT);
                INSERT INTO [authentication].[UserRole] ([Name])
                OUTPUT INSERTED.[Id] INTO @Ids
                VALUES (@Name);

                DECLARE @Id INT;
                SELECT @Id = [Id]
                FROM @Ids;

                INSERT INTO [authentication].[UserRolePermission] ([UserRoleId], [PermissionGroupId], [Permissions])
                SELECT @Id, [PermissionGroupId], [Permissions]
                FROM @Permissions;
            ");
        }

        public async Task UpdateUserRole(IOperation operation, UserRoleWithPermissionsEntity entity)
        {
            await operation.Connection.ExecuteAsync(new
            {
                entity.Id,
                entity.Name,
                Permissions = new UserRolePermissionEntities(entity.Permissions).ToTable()
            }, @"
                UPDATE [authentication].[UserRole]
                SET [Name] = @Name
                WHERE [Id] = @Id;

                MERGE [authentication].[UserRolePermission] t
                USING @Permissions s
                ON t.[UserRoleId] = @Id AND t.[PermissionGroupId] = s.[PermissionGroupId]
                WHEN MATCHED THEN
                    UPDATE SET t.[Permissions] = s.[Permissions]
                WHEN NOT MATCHED THEN
                    INSERT ([UserRoleId], [PermissionGroupId], [Permissions])
                    VALUES (@Id, s.[PermissionGroupId], s.[Permissions]);
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

        public async Task<IEnumerable<PermissionGroupEntity>> GetUserRolePermissions(IOperation operation)
        {
            var reader = await operation.Connection.QueryMultipleAsync(@"
                SELECT
                    [Id],
                    [PermissionGroupId],
                    [Name]
                FROM [enum].[Permission];

                SELECT
                    [Id],
                    [Name]
                FROM [enum].[PermissionGroup];
            ");
            var permissions = await reader.ReadAsync<PermissionExtendedEntity>();
            var permissionGroups = await reader.ReadAsync<PermissionGroupShortEntity>();
            return permissionGroups.Select(group => new PermissionGroupEntity
            {
                Id = group.Id,
                Name = group.Name,
                Permissions = permissions.Where(item => item.PermissionGroupId == group.Id)
            });
        }
    }
}