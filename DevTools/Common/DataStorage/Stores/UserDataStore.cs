using System.Threading.Tasks;
using MongoDB.Driver;
using VXDesign.Store.DevTools.Common.DataStorage.Entities;

namespace VXDesign.Store.DevTools.Common.DataStorage.Stores
{
    public interface IUserDataStore
    {
        #region Autorization

        Task<UserAuthorizationEntity> GetAuthorizationEntityById(string id);
        Task<string> GetRefreshTokenById(string id);
        Task<string> GetUserIdByAccessData(string email, string password = null);
        Task UpdateRefreshTokenById(string id, string refreshToken);
        Task<UserAuthorizationEntity> Create(UserRegistrationEntity entity);

        #endregion

        #region Users

        Task<FullUserDataEntity> GetUserByEmail(string email);

        #endregion
    }

    public class UserDataStore : BaseDataStore, IUserDataStore
    {
        private IMongoCollection<UserAuthorizationEntity> Authorizations { get; }
        private IMongoCollection<UserRegistrationEntity> Registrations { get; }
        private IMongoCollection<FullUserDataEntity> Users { get; }

        public UserDataStore(IMongoDatabase client) : base(client)
        {
            const string collection = "Users";
            Authorizations = Client.GetCollection<UserAuthorizationEntity>(collection);
            Registrations = Client.GetCollection<UserRegistrationEntity>(collection);
            Users = Client.GetCollection<FullUserDataEntity>(collection);
        }

        public async Task<UserAuthorizationEntity> GetAuthorizationEntityById(string id) => (await Authorizations.FindAsync(user => user.Id == id)).FirstOrDefault();

        public async Task<string> GetRefreshTokenById(string id) => (await GetAuthorizationEntityById(id))?.RefreshToken;

        public async Task<string> GetUserIdByAccessData(string email, string password = null)
        {
            return (await Registrations.FindAsync(user => user.Email == email.ToLowerInvariant() && (password == null || user.Password == password))).FirstOrDefault()?.Id;
        }

        public async Task UpdateRefreshTokenById(string id, string refreshToken)
        {
            await Authorizations.UpdateOneAsync(user => user.Id == id, Builders<UserAuthorizationEntity>.Update.Set(entity => entity.RefreshToken, refreshToken));
        }

        public async Task<UserAuthorizationEntity> Create(UserRegistrationEntity entity)
        {
            entity.Email = entity.Email.ToLowerInvariant();
            await Registrations.InsertOneAsync(entity);
            return await GetAuthorizationEntityById(entity.Id);
        }

        public async Task<FullUserDataEntity> GetUserByEmail(string email) => (await Users.FindAsync(user => user.Email == email.ToLowerInvariant())).FirstOrDefault();
    }
}