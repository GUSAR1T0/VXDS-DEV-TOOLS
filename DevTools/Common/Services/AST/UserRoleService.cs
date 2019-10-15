using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.DataStorage.Entities;
using VXDesign.Store.DevTools.Common.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services.AST
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleEntity>> GetUserRoles();
        Task AddUserRole(UserRoleEntity entity);
        Task UpdateUserRole(UserRoleEntity entity);
        Task DeleteUserRoleById(string id);
    }

    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleStore userRoleStore;

        public UserRoleService(IUserRoleStore userRoleStore)
        {
            this.userRoleStore = userRoleStore;
        }

        public async Task<IEnumerable<UserRoleEntity>> GetUserRoles() => await userRoleStore.GetUserRoles();

        public async Task AddUserRole(UserRoleEntity entity) => await userRoleStore.AddUserRole(entity);

        public async Task UpdateUserRole(UserRoleEntity entity) => await userRoleStore.UpdateUserRole(entity);

        public async Task DeleteUserRoleById(string id) => await userRoleStore.DeleteUserRoleById(id);
    }
}