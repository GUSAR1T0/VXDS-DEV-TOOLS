using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.User;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface IUserService
    {
        Task<UserPagingResponse> GetUsers(IOperation operation, UserPagingRequest request);
        Task<IEnumerable<UserShortEntity>> SearchUsersByPattern(IOperation operation, string pattern);
        Task<UserProfileEntity> GetUserProfileById(IOperation operation, int id);
        Task UpdateUserProfileGeneralInfo(IOperation operation, UserProfileEntity entity);
        Task UpdateUserProfileAccountSpecificInfo(IOperation operation, UserProfileEntity entity);
        Task ManageUserStatusById(IOperation operation, int id, bool status);
        Task<int> GetAffectedUsersCount(IOperation operation, int userRoleId);
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

        public async Task<UserPagingResponse> GetUsers(IOperation operation, UserPagingRequest request)
        {
            var (total, users) = await userDataStore.GetUsers(operation, request);
            return new UserPagingResponse
            {
                Total = total,
                Items = users
            };
        }

        public async Task<IEnumerable<UserShortEntity>> SearchUsersByPattern(IOperation operation, string pattern) => await userDataStore.SearchUsersByPattern(operation, pattern);

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
                throw CommonExceptions.CouldNotChangeOwnAccountSpecificInfo(operation);
            }

            await userDataStore.UpdateProfileAccountSpecificInfo(operation, entity);
        }

        public async Task ManageUserStatusById(IOperation operation, int id, bool status)
        {
            if (!await userDataStore.IsUserExist(operation, id))
            {
                throw CommonExceptions.UserWasNotFound(operation, id);
            }

            if (operation.OperationContext.UserId == id)
            {
                throw CommonExceptions.CouldNotChangeOwnAccountSpecificInfo(operation);
            }

            await userDataStore.ManageUserStatusById(operation, id, status);
        }

        public async Task<int> GetAffectedUsersCount(IOperation operation, int userRoleId) => await userDataStore.GetAffectedUsersCount(operation, userRoleId);
    }
}