using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.DataStorage.Entities;
using VXDesign.Store.DevTools.Common.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services.Base
{
    public interface IUserService
    {
        Task<FullUserDataEntity> GetUserByEmail(string email);
    }

    public class UserService : IUserService
    {
        private readonly IUserDataStore userDataStore;

        public UserService(IUserDataStore userDataStore)
        {
            this.userDataStore = userDataStore;
        }

        public async Task<FullUserDataEntity> GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            return await userDataStore.GetUserByEmail(email);
        }
    }
}