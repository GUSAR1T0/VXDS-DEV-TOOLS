using System.Threading.Tasks;
using MongoDB.Driver;
using VXDesign.Store.DevTools.Common.DataStorage.Entities;

namespace VXDesign.Store.DevTools.Common.DataStorage.Stores
{
    public interface IUserDataStore
    {
        #region Autorization

        Task<UserAuthorizationEntity> GetAuthorizationById(string id);
        Task<string> GetRefreshTokenById(string id);
        Task<string> GetIdByAccessData(string email, string password = null);
        Task UpdateRefreshTokenById(string id, string refreshToken);
        Task<UserAuthorizationEntity> CreateUser(UserRegistrationEntity entity);

        #endregion

        #region Users

        Task<bool> IsUserExist(string id);
        Task<UserProfileEntity> GetProfileByEmail(string email);
        Task UpdateProfile(UserProfileEntity entity);

        #endregion
    }

    public class UserDataStore : BaseDataStore, IUserDataStore
    {
        private IMongoCollection<UserAuthorizationEntity> Authorizations { get; }
        private IMongoCollection<UserRegistrationEntity> Registrations { get; }
        private IMongoCollection<UserProfileEntity> Users { get; }

        public UserDataStore(IMongoDatabase client) : base(client)
        {
            const string collection = "Users";
            Authorizations = Client.GetCollection<UserAuthorizationEntity>(collection);
            Registrations = Client.GetCollection<UserRegistrationEntity>(collection);
            Users = Client.GetCollection<UserProfileEntity>(collection);
        }

        #region Autorization

        public async Task<UserAuthorizationEntity> GetAuthorizationById(string id) => (await Authorizations.FindAsync(user => user.Id == id)).FirstOrDefault();

        public async Task<string> GetRefreshTokenById(string id) => (await GetAuthorizationById(id))?.RefreshToken;

        public async Task<string> GetIdByAccessData(string email, string password = null)
        {
            return (await Registrations.FindAsync(user => user.Email == email.ToLowerInvariant() && (password == null || user.Password == password))).FirstOrDefault()?.Id;
        }

        public async Task UpdateRefreshTokenById(string id, string refreshToken)
        {
            await Authorizations.UpdateOneAsync(user => user.Id == id, Builders<UserAuthorizationEntity>.Update.Set(user => user.RefreshToken, refreshToken));
        }

        public async Task<UserAuthorizationEntity> CreateUser(UserRegistrationEntity entity)
        {
            entity.Email = entity.Email.ToLowerInvariant();
            await Registrations.InsertOneAsync(entity);
            return await GetAuthorizationById(entity.Id);
        }

        #endregion

        #region Users

        public async Task<bool> IsUserExist(string id) => await Authorizations.Find(user => user.Id == id).AnyAsync();

        public async Task<UserProfileEntity> GetProfileByEmail(string email) => (await Users.FindAsync(user => user.Email == email.ToLowerInvariant())).FirstOrDefault();

        public async Task UpdateProfile(UserProfileEntity entity)
        {
            await Users.UpdateOneAsync(user => user.Id == entity.Id, Builders<UserProfileEntity>.Update
                .Set(user => user.FirstName, entity.FirstName)
                .Set(user => user.LastName, entity.LastName)
                .Set(user => user.Email, entity.Email)
                .Set(user => user.Color, entity.Color)
                .Set(user => user.Location, entity.Location)
                .Set(user => user.Bio, entity.Bio)
            );
        }

        #endregion
    }
}