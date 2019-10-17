using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Entities.Storage;
using VXDesign.Store.DevTools.Common.Storage.DataStores;

namespace VXDesign.Store.DevTools.Common.Services.Storage
{
    public interface IUserService
    {
        Task<UserProfileEntity> GetUserProfileByEmail(string email);
        Task UpdateUserProfile(UserProfileEntity entity);
    }

    public class UserService : IUserService
    {
        private readonly IUserDataStore userDataStore;

        public UserService(IUserDataStore userDataStore)
        {
            this.userDataStore = userDataStore;
        }

        public async Task<UserProfileEntity> GetUserProfileByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw CommonExceptions.FailedToGetProfileDueToMissedEmail();
            }

            var entity = await userDataStore.GetProfileByEmail(email);
            if (entity == null)
            {
                throw CommonExceptions.UserWasNotFound(email);
            }

            return entity;
        }

        public async Task UpdateUserProfile(UserProfileEntity entity)
        {
            if (!await userDataStore.IsUserExist(entity.Id))
            {
                throw CommonExceptions.UserWasNotFound();
            }

            await userDataStore.UpdateProfile(entity);
        }
    }
}