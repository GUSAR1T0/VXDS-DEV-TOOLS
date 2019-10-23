using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Storage;
using VXDesign.Store.DevTools.Common.Storage.DataStores;

namespace VXDesign.Store.DevTools.Common.Services.Storage
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleEntity>> GetUserRoles(IOperation operation, bool isFullInfoNeeded = true);
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

        public async Task AddUserRole(IOperation operation, UserRoleEntity entity) => await userRoleStore.AddUserRole(operation, entity);

        public async Task UpdateUserRole(IOperation operation, UserRoleEntity entity) => await userRoleStore.UpdateUserRole(operation, entity);

        public async Task DeleteUserRoleById(IOperation operation, int id) => await userRoleStore.DeleteUserRoleById(operation, id);
    }
}