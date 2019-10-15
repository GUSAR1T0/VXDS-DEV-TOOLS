using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.DataStorage.Entities;
using VXDesign.Store.DevTools.Common.DataStorage.Stores;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;

namespace VXDesign.Store.DevTools.Common.Services.AST
{
    public interface IUserService
    {
        Task<UserProfileEntity> GetUserProfileByEmail(string email);
        Task UpdateUserProfile(UserProfileEntity entity);
    }

    public class UserService : IUserService
    {
        private readonly IUserDataStore userDataStore;
        private readonly IUserRoleStore userRoleStore;

        public UserService(IUserDataStore userDataStore, IUserRoleStore userRoleStore)
        {
            this.userDataStore = userDataStore;
            this.userRoleStore = userRoleStore;
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

            if (!string.IsNullOrWhiteSpace(entity.RoleId))
            {
                entity.Role = await userRoleStore.GetUserRoleById(entity.RoleId);
            }

            return entity;
        }

        public async Task UpdateUserProfile(UserProfileEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                throw CommonExceptions.FailedToUpdateProfileDueToMissedId();
            }

            if (!await userDataStore.IsUserExist(entity.Id))
            {
                throw CommonExceptions.UserWasNotFound();
            }

            await userDataStore.UpdateProfile(entity);
        }
    }
}