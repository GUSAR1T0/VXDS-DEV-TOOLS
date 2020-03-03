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
        Task UpdateUserProfile(IOperation operation, UserProfileEntity entity);
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

        public async Task UpdateUserProfile(IOperation operation, UserProfileEntity entity)
        {
            if (!await userDataStore.IsUserExist(operation, entity.Id))
            {
                throw CommonExceptions.UserWasNotFound(operation, entity.Id);
            }

            await userDataStore.UpdateProfile(operation, entity);
        }

        public async Task<int> GetAffectedUsersCount(IOperation operation, int userRoleId) => await userDataStore.GetAffectedUsersCount(operation, userRoleId);
    }
}