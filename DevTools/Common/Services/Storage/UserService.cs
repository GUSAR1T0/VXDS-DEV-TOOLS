using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Storage;
using VXDesign.Store.DevTools.Common.Storage.DataStores;

namespace VXDesign.Store.DevTools.Common.Services.Storage
{
    public interface IUserService
    {
        Task<IEnumerable<UserListItem>> GetUsers(IOperation operation);
        Task<UserProfileEntity> GetUserProfileById(IOperation operation, int id);
        Task UpdateUserProfileGeneralInfo(IOperation operation, UserProfileEntity entity);
        Task UpdateUserProfileAccountSpecificInfo(IOperation operation, UserProfileEntity entity);
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

        public async Task<IEnumerable<UserListItem>> GetUsers(IOperation operation) => await userDataStore.GetUsers(operation);

        public async Task<UserProfileEntity> GetUserProfileById(IOperation operation, int id)
        {
            var entity = await userDataStore.GetProfileById(operation, id);
            if (entity == null)
            {
                throw CommonExceptions.UserWasNotFound(operation, id);
            }

            if (entity.UserRoleId != null)
            {
                entity.UserRole = await userRoleStore.GetUserRoleById(operation, entity.UserRoleId.Value);
            }

            return entity;
        }

        public async Task UpdateUserProfileGeneralInfo(IOperation operation, UserProfileEntity entity)
        {
            if (!await userDataStore.IsUserExist(operation, entity.Id))
            {
                throw CommonExceptions.UserWasNotFound(operation, entity.Id);
            }

            await userDataStore.UpdateProfileGeneralInfo(operation, entity);
        }

        public async Task UpdateUserProfileAccountSpecificInfo(IOperation operation, UserProfileEntity entity)
        {
            if (!await userDataStore.IsUserExist(operation, entity.Id))
            {
                throw CommonExceptions.UserWasNotFound(operation, entity.Id);
            }

            if (operation.OperationContext.UserId == entity.Id)
            {
                throw CommonExceptions.CouldNotChangeOwnUserRole(operation);
            }

            await userDataStore.UpdateProfileAccountSpecificInfo(operation, entity);
        }
    }
}