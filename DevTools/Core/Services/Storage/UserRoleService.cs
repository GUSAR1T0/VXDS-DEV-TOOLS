using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Core.Entities.Exceptions;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Entities.Storage.User;
using VXDesign.Store.DevTools.Core.Storage.DataStores;

namespace VXDesign.Store.DevTools.Core.Services.Storage
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation, bool isFullInfoNeeded = true);
        Task<UserRoleEntity> GetUserRoleById(IOperation operation, int id);
        Task AddUserRole(IOperation operation, UserRoleEntity entity);
        Task UpdateUserRole(IOperation operation, UserRoleEntity entity);
        Task DeleteUserRoleById(IOperation operation, int id);
    }

    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleStore userRoleStore;

        public UserRoleService(IUserRoleStore userRoleStore)
        {
            this.userRoleStore = userRoleStore;
        }

        public async Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation, bool isFullInfoNeeded = true) => await userRoleStore.GetUserRoles(operation, isFullInfoNeeded);

        public async Task<UserRoleEntity> GetUserRoleById(IOperation operation, int id) => await userRoleStore.GetUserRoleById(operation, id);

        public async Task AddUserRole(IOperation operation, UserRoleEntity entity)
        {
            if (await userRoleStore.IsUserRoleExist(operation, entity.Name))
            {
                throw CommonExceptions.UserRoleHasAlreadyExisted(operation, entity.Name);
            }

            await userRoleStore.AddUserRole(operation, entity);
        }

        public async Task UpdateUserRole(IOperation operation, UserRoleEntity entity)
        {
            if (!await userRoleStore.IsUserRoleExist(operation, entity.Id))
            {
                throw CommonExceptions.UserRoleWasNotFound(operation, entity.Id);
            }

            if (await userRoleStore.IsUserRoleExist(operation, entity.Name, entity.Id))
            {
                throw CommonExceptions.UserRoleHasAlreadyExisted(operation, entity.Name);
            }

            await userRoleStore.UpdateUserRole(operation, entity);
        }

        public async Task DeleteUserRoleById(IOperation operation, int id)
        {
            if (!await userRoleStore.IsUserRoleExist(operation, id))
            {
                throw CommonExceptions.UserRoleWasNotFound(operation, id);
            }

            await userRoleStore.DeleteUserRoleById(operation, id);
        }
    }
}