using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using VXDesign.Store.DevTools.Common.DataStorage.Entities;

namespace VXDesign.Store.DevTools.Common.DataStorage.Stores
{
    public interface IUserRoleStore
    {
        Task<UserRoleEntity> GetUserRoleById(string id);
        Task<IEnumerable<UserRoleEntity>> GetUserRoles();
        Task AddUserRole(UserRoleEntity entity);
        Task UpdateUserRole(UserRoleEntity entity);
        Task DeleteUserRoleById(string id);
    }

    public class UserRoleStore : BaseDataStore, IUserRoleStore
    {
        private IMongoCollection<UserRoleEntity> UserRoles { get; }

        public UserRoleStore(IMongoDatabase client) : base(client)
        {
            const string collection = "UserRoles";
            UserRoles = client.GetCollection<UserRoleEntity>(collection);
        }

        public async Task<UserRoleEntity> GetUserRoleById(string id) => await UserRoles.Find(userRole => userRole.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<UserRoleEntity>> GetUserRoles() => await UserRoles.Find(Builders<UserRoleEntity>.Filter.Empty).ToListAsync();

        public async Task AddUserRole(UserRoleEntity entity) => await UserRoles.InsertOneAsync(entity);

        public async Task UpdateUserRole(UserRoleEntity entity)
        {
            await UserRoles.UpdateOneAsync(userRole => userRole.Id == entity.Id, Builders<UserRoleEntity>.Update
                .Set(userRole => userRole.Name, entity.Name)
            );
        }

        public async Task DeleteUserRoleById(string id) => await UserRoles.DeleteOneAsync(userRole => userRole.Id == id);
    }
}