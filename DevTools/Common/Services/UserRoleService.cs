using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Permission;
using VXDesign.Store.DevTools.Common.Core.Entities.User;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation);
        Task<IEnumerable<UserRoleWithPermissionsEntity>> GetUserRolesWithPermissions(IOperation operation);
        Task<IEnumerable<UserRoleEntity>> SearchUserRolesByPattern(IOperation operation, string pattern);
        Task<UserRoleWithPermissionsEntity> GetUserRoleById(IOperation operation, int id);
        Task AddUserRole(IOperation operation, UserRoleWithPermissionsEntity entity);
        Task UpdateUserRole(IOperation operation, UserRoleWithPermissionsEntity entity);
        Task DeleteUserRoleById(IOperation operation, int id);
        Task<IEnumerable<PermissionGroupEntity>> GetUserRolePermissions(IOperation operation);
    }

    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleStore userRoleStore;

        public UserRoleService(IUserRoleStore userRoleStore)
        {
            this.userRoleStore = userRoleStore;
        }

        public async Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation) => await userRoleStore.GetUserRoles(operation);

        public async Task<IEnumerable<UserRoleWithPermissionsEntity>> GetUserRolesWithPermissions(IOperation operation) => await userRoleStore.GetUserRolesWithPermissions(operation);

        public async Task<IEnumerable<UserRoleEntity>> SearchUserRolesByPattern(IOperation operation, string pattern) => await userRoleStore.SearchUserRolesByPattern(operation, pattern);

        public async Task<UserRoleWithPermissionsEntity> GetUserRoleById(IOperation operation, int id) => await userRoleStore.GetUserRoleById(operation, id);

        public async Task AddUserRole(IOperation operation, UserRoleWithPermissionsEntity entity)
        {
            if (await userRoleStore.IsUserRoleExist(operation, entity.Name))
            {
                throw CommonExceptions.UserRoleHasAlreadyExisted(operation, entity.Name);
            }

            await userRoleStore.AddUserRole(operation, entity);
        }

        public async Task UpdateUserRole(IOperation operation, UserRoleWithPermissionsEntity entity)
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

        public async Task<IEnumerable<PermissionGroupEntity>> GetUserRolePermissions(IOperation operation) => await userRoleStore.GetUserRolePermissions(operation);
    }
}