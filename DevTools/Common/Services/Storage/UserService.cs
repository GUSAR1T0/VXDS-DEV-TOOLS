using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Storage;
using VXDesign.Store.DevTools.Common.Storage.DataStores;

namespace VXDesign.Store.DevTools.Common.Services.Storage
{
    public interface IUserService
    {
        Task<UserProfileEntity> GetUserProfileByEmail(IOperation operation, string email);
        Task UpdateUserProfile(IOperation operation, UserProfileEntity entity);
    }

    public class UserService : IUserService
    {
        private readonly IUserDataStore userDataStore;

        public UserService(IUserDataStore userDataStore)
        {
            this.userDataStore = userDataStore;
        }

        public async Task<UserProfileEntity> GetUserProfileByEmail(IOperation operation, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw CommonExceptions.FailedToGetProfileDueToMissedEmail();
            }

            var entity = await userDataStore.GetProfileByEmail(operation, email);
            if (entity == null)
            {
                throw CommonExceptions.UserWasNotFound(email);
            }

            return entity;
        }

        public async Task UpdateUserProfile(IOperation operation, UserProfileEntity entity)
        {
            if (!await userDataStore.IsUserExist(operation, entity.Id))
            {
                throw CommonExceptions.UserWasNotFound();
            }

            await userDataStore.UpdateProfile(operation, entity);
        }
    }
}