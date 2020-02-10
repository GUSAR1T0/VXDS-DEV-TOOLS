using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Entities.Storage.User;

namespace VXDesign.Store.DevTools.Core.Storage.DataStores
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
        Task<IEnumerable<UserListItem>> GetUsers(IOperation operation);
        Task<IEnumerable<UserShortEntity>> SearchUsersByPattern(IOperation operation, string pattern);
        Task<UserProfileEntity> GetProfileById(IOperation operation, int id);
        Task UpdateProfileGeneralInfo(IOperation operation, UserProfileEntity entity);
        Task UpdateProfileAccountSpecificInfo(IOperation operation, UserProfileEntity entity);
        Task ManageUserStatusById(IOperation operation, int id, bool status);
        Task<int> GetAffectedUsersCount(IOperation operation, int userRoleId);

        #endregion
    }

    public class UserDataStore : BaseDataStore, IUserDataStore
    {
        #region Autorization

        public async Task<UserAuthorizationEntity> GetAuthorizationById(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<UserAuthorizationEntity>(new { Id = id }, @"
                SELECT
                    au.[Id],
                    au.[FirstName],
                    au.[LastName],
                    au.[Email],
                    au.[Color],
                    aur.[PortalPermissions]
                FROM [authentication].[User] au
                LEFT JOIN [authentication].[UserRole] aur ON aur.[Id] = au.[UserRoleId]
                WHERE au.[Id] = @Id
            ");
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
            return await operation.Connection.QuerySingleOrDefaultAsync<UserAuthorizationEntity>(new
            {
                Email = email,
                Password = password
            }, @"
                SELECT
                    au.[Id],
                    aur.[PortalPermissions]
                FROM [authentication].[User] au
                LEFT JOIN [authentication].[UserRole] aur ON aur.[Id] = au.[UserRoleId]
                WHERE [Email] = @Email AND (@Password IS NULL OR [Password] = @Password)
            ");
        }

        public async Task<UserAuthorizationEntity> GetUserIdentityClaimsById(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<UserAuthorizationEntity>(new { Id = id }, @"
                SELECT
                    au.[Id],
                    aur.[PortalPermissions]
                FROM [authentication].[User] au
                LEFT JOIN [authentication].[UserRole] aur ON aur.[Id] = au.[UserRoleId]
                WHERE au.[Id] = @Id
            ");
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

        public async Task<IEnumerable<UserListItem>> GetUsers(IOperation operation)
        {
            return await operation.Connection.QueryAsync<UserListItem>(@"
                SELECT
                    au.[Id],
                    [FirstName],
                    [LastName],
                    [Email],
                    [Color],
                    aur.[Name] AS [UserRole],
                    [IsActivated]
                FROM [authentication].[User] au
                LEFT JOIN [authentication].[UserRole] aur ON au.[UserRoleId] = aur.[Id]
            ");
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

        public async Task UpdateProfileGeneralInfo(IOperation operation, UserProfileEntity entity)
        {
            await operation.Connection.ExecuteAsync(new
            {
                entity.Id,
                entity.FirstName,
                entity.LastName,
                entity.Email,
                entity.Color,
                entity.Location,
                entity.Bio
            }, @"
                UPDATE [authentication].[User]
                SET
                    [FirstName] = @FirstName,
                    [LastName] = @LastName,
                    [Email] = @Email,
                    [Color] = @Color,
                    [Location] = @Location,
                    [Bio] = @Bio
                WHERE [Id] = @Id
            ");
        }

        public async Task UpdateProfileAccountSpecificInfo(IOperation operation, UserProfileEntity entity)
        {
            await operation.Connection.ExecuteAsync(new
            {
                entity.Id,
                entity.UserRoleId,
                entity.IsActivated
            }, @"
                UPDATE [authentication].[User]
                SET
                    [UserRoleId] = @UserRoleId,
                    [IsActivated] = @IsActivated
                WHERE [Id] = @Id
            ");
        }

        public async Task ManageUserStatusById(IOperation operation, int id, bool status)
        {
            await operation.Connection.ExecuteAsync(new
            {
                Id = id,
                Status = status
            }, @"
                UPDATE [authentication].[User]
                SET [IsActivated] = @Status
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