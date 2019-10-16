using System.Threading.Tasks;
using Dapper;
using Npgsql;
using VXDesign.Store.DevTools.Common.Containers.Properties;
using VXDesign.Store.DevTools.Common.DataStorage.Entities;

namespace VXDesign.Store.DevTools.Common.DataStorage.Stores
{
    public interface IUserDataStore
    {
        #region Autorization

        Task<UserAuthorizationEntity> GetAuthorizationById(int id);
        Task<string> GetRefreshTokenById(int id);
        Task<int?> GetIdByAccessData(string email, string password = null);
        Task UpdateRefreshTokenById(int id, string refreshToken);
        Task<UserAuthorizationEntity> CreateUser(UserRegistrationEntity entity);

        #endregion

        #region Users

        Task<bool> IsUserExist(int id);
        Task<UserProfileEntity> GetProfileByEmail(string email);
        Task UpdateProfile(UserProfileEntity entity);

        #endregion
    }

    public class UserDataStore : BaseDataStore, IUserDataStore
    {
        public UserDataStore(DatabaseConnectionProperties properties) : base(properties)
        {
        }

        public async Task<UserAuthorizationEntity> GetAuthorizationById(int id)
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<UserAuthorizationEntity>(@"
                    SELECT
                        ""Id"",
                        ""FirstName"",
                        ""LastName"",
                        ""Email"",
                        ""Color""
                    FROM ""authorization"".""User""
                    WHERE ""Id"" = @Id
                ", new { Id = id });
            }
        }

        public async Task<string> GetRefreshTokenById(int id)
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<string>(@"
                    SELECT ""RefreshToken""
                    FROM ""authorization"".""User""
                    WHERE ""Id"" = @Id
                ", new { Id = id });
            }
        }

        public async Task<int?> GetIdByAccessData(string email, string password = null)
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<int?>(@"
                    SELECT ""Id""
                    FROM ""authorization"".""User""
                    WHERE ""Email"" = @Email AND (@Password IS NULL OR ""Password"" = @Password)
                ", new { Email = email, Password = password });
            }
        }

        public async Task UpdateRefreshTokenById(int id, string refreshToken)
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(@"
                    UPDATE ""authorization"".""User""
                    SET ""RefreshToken"" = @RefreshToken
                    WHERE ""Id"" = @Id
                ", new { Id = id, RefreshToken = refreshToken });
            }
        }

        public async Task<UserAuthorizationEntity> CreateUser(UserRegistrationEntity entity)
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                var id = await connection.QuerySingleAsync<int>(@"
                    INSERT INTO ""base"".""User"" (""FirstName"", ""LastName"", ""Email"", ""Password"", ""Color"")
                    VALUES (@FirstName, @LastName, @Email, @Password, @Color)
                    RETURNING ""Id""
                ", new { entity.FirstName, entity.LastName, entity.Email, entity.Password, entity.Color });
                return await connection.QueryFirstOrDefaultAsync<UserAuthorizationEntity>(@"
                    SELECT
                        ""Id"",
                        ""FirstName"",
                        ""LastName"",
                        ""Email"",
                        ""Color""
                    FROM ""authorization"".""User""
                    WHERE ""Id"" = @Id
                ", new { Id = id });
            }
        }

        public async Task<bool> IsUserExist(int id)
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<bool>(@"
                    SELECT 1
                    FROM ""authorization"".""User""
                    WHERE ""Id"" = @Id
                ", new { Id = id });
            }
        }

        public async Task<UserProfileEntity> GetProfileByEmail(string email)
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<UserProfileEntity>(@"
                    SELECT
                        ""Id"",
                        ""FirstName"",
                        ""LastName"",
                        ""Email"",
                        ""Color"",
                        ""Location"",
                        ""Bio""
                    FROM ""authorization"".""User""
                    WHERE ""Email"" = @Email
                ", new { Email = email });
            }
        }

        public async Task UpdateProfile(UserProfileEntity entity)
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(@"
                    UPDATE ""authorization"".""User""
                    SET
                        ""FirstName"" = @FirstName,
                        ""LastName"" = @LastName,
                        ""Email"" = @Email,
                        ""Color"" = @Color,
                        ""Location"" = @Location,
                        ""Bio"" = @Bio
                    WHERE ""Id"" = @Id
                ", new { entity.Id, entity.FirstName, entity.LastName, entity.Email, entity.Color, entity.Location, entity.Bio });
            }
        }
    }
}