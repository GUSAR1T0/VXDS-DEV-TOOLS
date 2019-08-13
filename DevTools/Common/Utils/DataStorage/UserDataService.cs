using System.Threading.Tasks;
using MongoDB.Driver;
using VXDesign.Store.DevTools.Common.Entities.DataStorage;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Utils.DataStorage
{
    public interface IUserDataService
    {
        Task<UserAuthorizationEntity> GetEntityById(string id);
        Task<string> GetRefreshTokenById(string id);
        Task<string> GetIdByUser(string email, string password = null);
        Task UpdateRefreshToken(string id, string refreshToken);
        Task<UserAuthorizationEntity> Create(UserRegistrationEntity entity);
    }

    public class UserDataService : BaseDataService, IUserDataService
    {
        private IMongoCollection<UserAuthorizationEntity> Authorizations { get; }
        private IMongoCollection<UserRegistrationEntity> Registrations { get; }

        public UserDataService(DatabaseConnectionProperties properties) : base(properties)
        {
            const string collection = "Users";
            Authorizations = Client.GetCollection<UserAuthorizationEntity>(collection);
            Registrations = Client.GetCollection<UserRegistrationEntity>(collection);
        }

        public async Task<UserAuthorizationEntity> GetEntityById(string id)
        {
            var filter = await Authorizations.FindAsync(user => user.Id == id);
            return filter.FirstOrDefault();
        }

        public async Task<string> GetRefreshTokenById(string id)
        {
            return (await GetEntityById(id))?.RefreshToken;
        }

        public async Task<string> GetIdByUser(string email, string password = null)
        {
            var filter = await Registrations.FindAsync(user => user.Email == email && (password == null || user.Password == password));
            return filter.FirstOrDefault()?.Id;
        }

        public async Task UpdateRefreshToken(string id, string refreshToken)
        {
            await Authorizations.UpdateOneAsync(user => user.Id == id, Builders<UserAuthorizationEntity>.Update.Set(entity => entity.RefreshToken, refreshToken));
        }

        public async Task<UserAuthorizationEntity> Create(UserRegistrationEntity entity)
        {
            await Registrations.InsertOneAsync(entity);
            return await GetEntityById(entity.Id);
        }
    }
}