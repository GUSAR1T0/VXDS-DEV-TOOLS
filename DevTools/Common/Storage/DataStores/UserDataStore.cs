using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Storage;

namespace VXDesign.Store.DevTools.Common.Storage.DataStores
{
    public interface IUserDataStore
    {
        #region Autorization

        Task<UserAuthorizationEntity> GetAuthorizationById(IOperation operation, int id);
        Task<string> GetRefreshTokenById(IOperation operation, int id);
        Task<UserAuthorizationEntity> GetUserIdentityClaimsByAccessData(IOperation operation, string email, string password = null);
        Task UpdateRefreshTokenById(IOperation operation, int id, string refreshToken);
        Task<UserAuthorizationEntity> CreateUser(IOperation operation, UserRegistrationEntity entity);

        #endregion

        #region Users

        Task<bool> IsUserExist(IOperation operation, int id);
        Task<IEnumerable<UserListItem>> GetUsers(IOperation operation);
        Task<UserProfileEntity> GetProfileById(IOperation operation, int id);
        Task UpdateProfileGeneralInfo(IOperation operation, UserProfileEntity entity);
        Task UpdateProfileAccountSpecificInfo(IOperation operation, UserProfileEntity entity);

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
                    aur.[UserPermissions],
                    aur.[UserRolePermissions]
                FROM [authorization].[User] au
                LEFT JOIN [authorization].[UserRole] aur ON aur.[Id] = au.[UserRoleId]
                WHERE au.[Id] = @Id
            ");
        }

        public async Task<string> GetRefreshTokenById(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<string>(new { Id = id }, @"
                SELECT [RefreshToken]
                FROM [authorization].[User]
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
                    aur.[UserPermissions],
                    aur.[UserRolePermissions]
                FROM [authorization].[User] au
                LEFT JOIN [authorization].[UserRole] aur ON aur.[Id] = au.[UserRoleId]
                WHERE [Email] = @Email AND (@Password IS NULL OR [Password] = @Password)
            ");
        }

        public async Task UpdateRefreshTokenById(IOperation operation, int id, string refreshToken)
        {
            await operation.Connection.ExecuteAsync(new
            {
                Id = id,
                RefreshToken = refreshToken
            }, @"
                UPDATE [authorization].[User]
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

                INSERT INTO [authorization].[User] ([FirstName], [LastName], [Email], [Password], [Color])
                OUTPUT INSERTED.[Id] INTO @Id
                VALUES (@FirstName, @LastName, @Email, @Password, @Color)

                SELECT
                    au.[Id],
                    [FirstName],
                    [LastName],
                    [Email],
                    [Color]
                FROM [authorization].[User] au
                INNER JOIN @Id i ON au.[Id] = i.[Id]
            ");
        }

        #endregion

        #region Users

        public async Task<bool> IsUserExist(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { Id = id }, @"
                SELECT 1
                FROM [authorization].[User]
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
                    aur.[Name] AS [UserRole]
                FROM [authorization].[User] au
                LEFT JOIN [authorization].[UserRole] aur ON au.[UserRoleId] = aur.[Id]
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
                    [UserRoleId]
                FROM [authorization].[User]
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
                UPDATE [authorization].[User]
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
                entity.UserRoleId
            }, @"
                UPDATE [authorization].[User]
                SET [UserRoleId] = @UserRoleId
                WHERE [Id] = @Id
            ");
        }

        #endregion
    }
}