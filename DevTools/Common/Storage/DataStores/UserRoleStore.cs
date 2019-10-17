using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using VXDesign.Store.DevTools.Common.Entities.Properties;
using VXDesign.Store.DevTools.Common.Entities.Storage;

namespace VXDesign.Store.DevTools.Common.Storage.DataStores
{
    public interface IUserRoleStore
    {
        Task<UserRoleEntity> GetUserRoleById(int id);
        Task<IEnumerable<UserRoleEntity>> GetUserRoles();
        Task AddUserRole(UserRoleEntity entity);
        Task UpdateUserRole(UserRoleEntity entity);
        Task DeleteUserRoleById(int id);
    }

    public class UserRoleStore : BaseDataStore, IUserRoleStore
    {
        public UserRoleStore(DatabaseConnectionProperties properties) : base(properties)
        {
        }

        public async Task<UserRoleEntity> GetUserRoleById(int id)
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<UserRoleEntity>(@"
                    SELECT
                        ""Id"",
                        ""Name""
                    FROM ""authorization"".""UserRole""
                    WHERE ""Id"" = @Id
                ", new { Id = id });
            }
        }

        public async Task<IEnumerable<UserRoleEntity>> GetUserRoles()
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<UserRoleEntity>(@"
                    SELECT
                        ""Id"",
                        ""Name""
                    FROM ""authorization"".""UserRole""
                ");
            }
        }

        public async Task AddUserRole(UserRoleEntity entity)
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(@"
                    INSERT INTO ""authorization"".""UserRole"" (""Name"")
                    VALUES (@Name)
                ", new { entity.Name });
            }
        }

        public async Task UpdateUserRole(UserRoleEntity entity)
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(@"
                    UPDATE ""authorization"".""UserRole""
                    SET ""Name"" = @Name
                    WHERE ""Id"" = @Id
                ", new { entity.Id, entity.Name });
            }
        }

        public async Task DeleteUserRoleById(int id)
        {
            using (var connection = new NpgsqlConnection(Properties.DataStoreConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(@"
                    DELETE FROM ""authorization"".""UserRole""
                    WHERE ""Id"" = @Id
                ", new { Id = id });
            }
        }
    }
}