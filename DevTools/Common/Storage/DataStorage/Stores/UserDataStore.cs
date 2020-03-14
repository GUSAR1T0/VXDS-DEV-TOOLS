using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities.User;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Extensions;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IUserDataStore
    {
        #region Autorization

        Task<UserAuthorizationEntity> GetAuthorizationById(IOperation operation, int id);
        Task<string> GetRefreshTokenById(IOperation operation, int id);
        Task<UserAuthorizationEntity> GetUserIdentityClaimsByAccessData(IOperation operation, string email, string password = null);
        Task<UserAuthorizationEntity> GetUserIdentityClaimsById(IOperation operation, int id);
        Task UpdateRefreshTokenById(IOperation operation, int id, string refreshToken);
        Task<UserAuthorizationEntity> CreateUser(IOperation operation, UserRegistrationEntity entity);
        Task<bool> IsUserActivated(IOperation operation, int id);

        #endregion

        #region Users

        Task<bool> IsUserExist(IOperation operation, int id);
        Task<(long total, IEnumerable<UserListItem> users)> GetUsers(IOperation operation, UserPagingRequest request);
        Task<IEnumerable<UserShortEntity>> SearchUsersByPattern(IOperation operation, string pattern);
        Task<UserProfileEntity> GetProfileById(IOperation operation, int id);
        Task UpdateProfile(IOperation operation, UserProfileEntity entity);
        Task<int> GetAffectedUsersCount(IOperation operation, int userRoleId);

        #endregion
    }

    public class UserDataStore : BaseDataStore, IUserDataStore
    {
        #region Autorization

        public async Task<UserAuthorizationEntity> GetAuthorizationById(IOperation operation, int id)
        {
            var reader = await operation.Connection.QueryMultipleAsync(new { Id = id }, @"
                SELECT
                    au.[Id],
                    au.[FirstName],
                    au.[LastName],
                    au.[Email],
                    au.[Color],
                    au.[UserRoleId]
                FROM [authentication].[User] au
                WHERE au.[Id] = @Id;

                SELECT
                    aurp.[PermissionGroupId],
                    aurp.[Permissions]
                FROM [authentication].[UserRolePermission] aurp
                INNER JOIN [authentication].[User] au ON aurp.[UserRoleId] = au.[UserRoleId]
                WHERE au.[Id] = @Id;
            ");
            var user = await reader.ReadSingleOrDefaultAsync<UserAuthorizationShortEntity>();
            return user != null
                ? new UserAuthorizationEntity
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Color = user.Color,
                    Permissions = await reader.ReadAsync<UserRolePermissionEntity>()
                }
                : null;
        }

        public async Task<string> GetRefreshTokenById(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<string>(new { Id = id }, @"
                SELECT [RefreshToken]
                FROM [authentication].[User]
                WHERE [Id] = @Id
            ");
        }

        public async Task<UserAuthorizationEntity> GetUserIdentityClaimsByAccessData(IOperation operation, string email, string password = null)
        {
            var reader = await operation.Connection.QueryMultipleAsync(new
            {
                Email = email,
                Password = password
            }, @"
                DECLARE @Ids TABLE ([Id] INT);
                INSERT INTO @Ids ([Id])
                SELECT au.[Id]
                FROM [authentication].[User] au
                WHERE [Email] = @Email AND (@Password IS NULL OR [Password] = @Password);

                DECLARE @Id INT;
                SELECT @Id = [Id] FROM @Ids;

                SELECT @Id;

                SELECT
                    aurp.[PermissionGroupId],
                    aurp.[Permissions]
                FROM [authentication].[UserRolePermission] aurp
                INNER JOIN [authentication].[User] au ON aurp.[UserRoleId] = au.[UserRoleId]
                WHERE au.[Id] = @Id;
            ");
            var id = await reader.ReadSingleOrDefaultAsync<int?>();
            return id != null
                ? new UserAuthorizationEntity
                {
                    Id = id.Value,
                    Permissions = await reader.ReadAsync<UserRolePermissionEntity>()
                }
                : null;
        }

        public async Task<UserAuthorizationEntity> GetUserIdentityClaimsById(IOperation operation, int id)
        {
            var permissions = await operation.Connection.QueryAsync<UserRolePermissionEntity>(new { Id = id }, @"
                SELECT
                    aurp.[PermissionGroupId],
                    aurp.[Permissions]
                FROM [authentication].[UserRolePermission] aurp
                INNER JOIN [authentication].[User] au ON aurp.[UserRoleId] = au.[UserRoleId]
                WHERE au.[Id] = @Id;
            ");
            return new UserAuthorizationEntity
            {
                Id = id,
                Permissions = permissions
            };
        }

        public async Task UpdateRefreshTokenById(IOperation operation, int id, string refreshToken)
        {
            await operation.Connection.ExecuteAsync(new
            {
                Id = id,
                RefreshToken = refreshToken
            }, @"
                UPDATE [authentication].[User]
                SET [RefreshToken] = @RefreshToken
                WHERE [Id] = @Id
            ");
        }

        public async Task<UserAuthorizationEntity> CreateUser(IOperation operation, UserRegistrationEntity entity)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<UserAuthorizationEntity>(new
            {
                entity.FirstName,
                entity.LastName,
                entity.Email,
                entity.Password,
                entity.Color
            }, @"
                DECLARE @Id TABLE ([Id] INT)

                INSERT INTO [authentication].[User] ([FirstName], [LastName], [Email], [Password], [Color])
                OUTPUT INSERTED.[Id] INTO @Id
                VALUES (@FirstName, @LastName, @Email, @Password, @Color)

                SELECT
                    au.[Id],
                    [FirstName],
                    [LastName],
                    [Email],
                    [Color]
                FROM [authentication].[User] au
                INNER JOIN @Id i ON au.[Id] = i.[Id]
            ");
        }

        public async Task<bool> IsUserActivated(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { Id = id }, @"
                SELECT TOP 1 1
                FROM [authentication].[User]
                WHERE [Id] = @Id AND [IsActivated] = 1
            ");
        }

        #endregion

        #region Users

        public async Task<bool> IsUserExist(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { Id = id }, @"
                SELECT TOP 1 1
                FROM [authentication].[User]
                WHERE [Id] = @Id
            ");
        }

        public async Task<(long total, IEnumerable<UserListItem> users)> GetUsers(IOperation operation, UserPagingRequest request)
        {
            var selectBase = $@"
                SELECT {{0}} FROM [authentication].[User] au
                LEFT JOIN [authentication].[UserRole] aur ON au.[UserRoleId] = aur.[Id]
                {{1}}
                {{2}}
            ";
            const string selectEntity = @"
                    au.[Id],
                    au.[FirstName],
                    au.[LastName],
                    au.[Email],
                    au.[Color],
                    au.[UserRoleId],
                    aur.[Name] AS [UserRole],
                    au.[IsActivated]
            ";
            const string selectTotal = "COUNT_BIG(1)";
            var (@params, joins, filters) = HandleGetRequest(request.Filter);
            var query = $@"
                {string.Format(selectBase, selectTotal, joins, filters)};
                {string.Format(selectBase, selectEntity, joins, filters)}
                ORDER BY au.[Id]
                OFFSET {request.PageNo * request.PageSize} ROWS FETCH NEXT {request.PageSize} ROWS ONLY;
            ";
            using var reader = await operation.Connection.QueryMultipleAsync(@params, query);
            var total = await reader.ReadSingleAsync<long>();
            var users = await reader.ReadAsync<UserListItem>();
            return (total, users);
        }

        private static (DynamicParameters, string, string) HandleGetRequest(UserPagingFilter filter)
        {
            var @params = new DynamicParameters();
            var joins = new List<string>();
            var filters = new List<string>();

            if (filter.Ids?.Any() == true)
            {
                @params.Add("Ids", filter.Ids);
                filters.Add("au.[Id] IN @Ids");
            }

            if (filter.UserNames?.Any() == true)
            {
                @params.Add("UserNames", filter.UserNames.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @UserNames un ON (au.[FirstName] + ' ' + au.[LastName]) LIKE un.[Value]");
            }

            if (filter.Emails?.Any() == true)
            {
                @params.Add("Emails", filter.Emails.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Emails e ON au.[Email] LIKE e.[Value]");
            }

            if (filter.UserRoleIds?.Any() == true)
            {
                @params.Add("UserRoleIds", filter.UserRoleIds);
                filters.Add("au.[UserRoleId] IN @UserRoleIds");
            }

            if (filter.IsActivated != null)
            {
                @params.Add("IsActivated", filter.IsActivated);
                filters.Add("au.[IsActivated] = @IsActivated");
            }

            return (@params, string.Join(" ", joins), filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }

        public async Task<IEnumerable<UserShortEntity>> SearchUsersByPattern(IOperation operation, string pattern)
        {
            return await operation.Connection.QueryAsync<UserShortEntity>(new { Pattern = $"%{pattern}%" }, @"
                SELECT
                    u.[Id],
                    u.[FullName]
                FROM (
                    SELECT
                        [Id],
                        ([FirstName] + ' ' + [LastName]) [FullName]
                    FROM [authentication].[User]
                    UNION ALL
                    SELECT 0, 'Unauthorized'
                ) u
                WHERE u.[FullName] LIKE @Pattern;
            ");
        }

        public async Task<UserProfileEntity> GetProfileById(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<UserProfileEntity>(new { Id = id }, @"
                SELECT
                    [Id],
                    [FirstName],
                    [LastName],
                    [Email],
                    [Color],
                    [Location],
                    [Bio],
                    [UserRoleId],
                    [IsActivated]
                FROM [authentication].[User]
                WHERE [Id] = @Id
            ");
        }

        public async Task UpdateProfile(IOperation operation, UserProfileEntity entity)
        {
            await operation.Connection.ExecuteAsync(new
            {
                entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.Email,
                entity.Color,
                entity.Location,
                entity.Bio,
                entity.UserRoleId,
                entity.IsActivated
            }, @"
                UPDATE [authentication].[User]
                SET
                    [FirstName] = @FirstName,
                    [LastName] = @LastName,
                    [Email] = @Email,
                    [Color] = @Color,
                    [Location] = @Location,
                    [Bio] = @Bio,
                    [UserRoleId] = @UserRoleId,
                    [IsActivated] = @IsActivated
                WHERE [Id] = @Id
            ");
        }

        public async Task<int> GetAffectedUsersCount(IOperation operation, int userRoleId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<int>(new { UserRoleId = userRoleId }, @"
                SELECT COUNT(*)
                FROM [authentication].[User]
                WHERE [UserRoleId] = @UserRoleId
            ");
        }

        #endregion
    }
}