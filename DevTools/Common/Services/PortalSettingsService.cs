using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Clients.GitHub;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Models.Users;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.GitHub;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface IPortalSettingsService
    {
        #region Hosts

        Task<HostPagingResponse> GetHosts(IOperation operation, HostPagingRequest request);
        Task AddHost(IOperation operation, HostSettingsItemEntity host);
        Task UpdateHost(IOperation operation, HostSettingsItemEntity host);
        Task DeleteHost(IOperation operation, int hostId);
        Task<int> GetAffectedModulesCount(IOperation operation, int hostId);

        #endregion

        #region Code Services

        Task<CodeServiceSettingsEntity> GetCodeServicesData(IOperation operation);
        Task<GitHubUserProfileEntity> SetupGitHubToken(IOperation operation, string token);
        Task<IGitHubClientService> GetGitHubClient(IOperation operation);

        #endregion
    }

    public class PortalSettingsService : IPortalSettingsService
    {
        private readonly IPortalSettingsStore portalSettingsStore;

        public PortalSettingsService(IPortalSettingsStore portalSettingsStore)
        {
            this.portalSettingsStore = portalSettingsStore;
        }

        #region Hosts

        public async Task<HostPagingResponse> GetHosts(IOperation operation, HostPagingRequest request)
        {
            var (total, hosts) = await portalSettingsStore.GetHosts(operation, request);
            return new HostPagingResponse
            {
                Total = total,
                Items = hosts
            };
        }

        public async Task AddHost(IOperation operation, HostSettingsItemEntity host) => await portalSettingsStore.AddHost(operation, host);

        public async Task UpdateHost(IOperation operation, HostSettingsItemEntity host)
        {
            if (!await portalSettingsStore.IsHostExist(operation, host.Id))
            {
                throw CommonExceptions.HostWasNotFound(operation, host.Id);
            }

            await portalSettingsStore.UpdateHost(operation, host);
        }

        public async Task DeleteHost(IOperation operation, int hostId)
        {
            if (!await portalSettingsStore.IsHostExist(operation, hostId))
            {
                throw CommonExceptions.HostWasNotFound(operation, hostId);
            }

            await portalSettingsStore.DeleteHost(operation, hostId);
        }

        public async Task<int> GetAffectedModulesCount(IOperation operation, int hostId)
        {
            // TODO: Implement later
            return 0;
        }

        #endregion

        #region Code Services

        public async Task<CodeServiceSettingsEntity> GetCodeServicesData(IOperation operation)
        {
            var parameters = await portalSettingsStore.GetSettingsParameters(operation, PortalSettingsKey.GitHubToken);
            return new CodeServiceSettingsEntity
            {
                GitHubUser = await GetGitHubUser(operation, parameters.FirstOrDefault()?.Value)
            };
        }

        private static async Task<GitHubUserProfileEntity> GetGitHubUser(IOperation operation, string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                var gitHubClient = new GitHubClientService(token);
                var user = await new Users.GetUserRequest().SendRequest(operation, gitHubClient);
                return user.IsWithoutErrors()
                    ? new GitHubUserProfileEntity
                    {
                        IsValid = true,
                        Login = user.Response.Login,
                        Name = user.Response.Name,
                        AvatarUrl = user.Response.AvatarUrl,
                        ProfileUrl = user.Response.HtmlUrl
                    }
                    : new GitHubUserProfileEntity
                    {
                        IsValid = false
                    };
            }

            return null;
        }

        public async Task<GitHubUserProfileEntity> SetupGitHubToken(IOperation operation, string token)
        {
            await portalSettingsStore.ModifySettings(operation, PortalSettingsKey.GitHubToken, token);
            var settingsParameter = await portalSettingsStore.GetSettingsParameter(operation, PortalSettingsKey.GitHubToken);
            return await GetGitHubUser(operation, settingsParameter);
        }

        public async Task<IGitHubClientService> GetGitHubClient(IOperation operation)
        {
            var token = await portalSettingsStore.GetSettingsParameter(operation, PortalSettingsKey.GitHubToken);
            return !string.IsNullOrWhiteSpace(token) ? new GitHubClientService(token) : throw CommonExceptions.GitHubTokenIsNotStated(operation);
        }

        #endregion
    }
}