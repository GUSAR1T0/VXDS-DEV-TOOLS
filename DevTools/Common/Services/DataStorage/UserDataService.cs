using System.Threading.Tasks;
using MongoDB.Driver;
using VXDesign.Store.DevTools.Common.Containers.DataStorage;
using VXDesign.Store.DevTools.Common.Containers.Properties;

namespace VXDesign.Store.DevTools.Common.Services.DataStorage
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

        public async Task<UserAuthorizationEntity> GetEntityById(string id) => (await Authorizations.FindAsync(user => user.Id == id)).FirstOrDefault();

        public async Task<string> GetRefreshTokenById(string id) => (await GetEntityById(id))?.RefreshToken;

        public async Task<string> GetIdByUser(string email, string password = null)
        {
            email = email.ToLowerInvariant();
            var filter = await Registrations.FindAsync(user => user.Email == email && (password == null || user.Password == password));
            return filter.FirstOrDefault()?.Id;
        }

        public async Task UpdateRefreshToken(string id, string refreshToken)
        {
            await Authorizations.UpdateOneAsync(user => user.Id == id, Builders<UserAuthorizationEntity>.Update.Set(entity => entity.RefreshToken, refreshToken));
        }

        public async Task<UserAuthorizationEntity> Create(UserRegistrationEntity entity)
        {
            entity.Email = entity.Email.ToLowerInvariant();
            await Registrations.InsertOneAsync(entity);
            return await GetEntityById(entity.Id);
        }
    }
}