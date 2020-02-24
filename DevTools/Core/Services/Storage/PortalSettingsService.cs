using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Core.Entities.Common;
using VXDesign.Store.DevTools.Core.Entities.Exceptions;
using VXDesign.Store.DevTools.Core.Entities.GitHub.Users.Models;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Entities.Storage.GitHub;
using VXDesign.Store.DevTools.Core.Entities.Storage.Settings;
using VXDesign.Store.DevTools.Core.Extensions.HTTP;
using VXDesign.Store.DevTools.Core.Services.GitHub;
using VXDesign.Store.DevTools.Core.Storage.DataStores;

namespace VXDesign.Store.DevTools.Core.Services.Storage
{
    public interface IPortalSettingsService
    {
        Task<SettingsParametersEntity> GetSettings(IOperation operation);

        #region GitHub

        Task<GitHubUserEntity> SetupGitHubToken(IOperation operation, string token);

        #endregion

        Task<IGitHubClientService> GetGitHubClient(IOperation operation);
    }

    public class PortalSettingsService : IPortalSettingsService
    {
        private readonly IPortalSettingsStore portalSettingsStore;

        public PortalSettingsService(IPortalSettingsStore portalSettingsStore)
        {
            this.portalSettingsStore = portalSettingsStore;
        }

        public async Task<SettingsParametersEntity> GetSettings(IOperation operation)
        {
            var parameters = await portalSettingsStore.GetSettingsParameters(operation);
            var settings = new SettingsParametersEntity
            {
                CodeServiceSettings = new CodeServiceSettingsEntity()
            };

            var token = parameters.FirstOrDefault(item => item.Name == PortalSettingsKey.GitHubToken);
            settings.CodeServiceSettings.GitHubUser = await GetGitHubUser(operation, token?.Value);

            return settings;
        }

        #region GitHub

        private static async Task<GitHubUserEntity> GetGitHubUser(IOperation operation, string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                var gitHubClient = new GitHubClientService(token);
                var user = await new Users.GetUserRequest().SendRequest(operation, gitHubClient);
                return user.IsWithoutErrors()
                    ? new GitHubUserEntity
                    {
                        IsValid = true,
                        Login = user.Response.Login,
                        Name = user.Response.Name,
                        AvatarUrl = user.Response.AvatarUrl,
                        ProfileUrl = user.Response.HtmlUrl
                    }
                    : new GitHubUserEntity
                    {
                        IsValid = false
                    };
            }

            return null;
        }

        public async Task<GitHubUserEntity> SetupGitHubToken(IOperation operation, string token)
        {
            await portalSettingsStore.ModifySettings(operation, PortalSettingsKey.GitHubToken, token);
            var settingsParameter = await portalSettingsStore.GetSettingsParameter(operation, PortalSettingsKey.GitHubToken);
            return await GetGitHubUser(operation, settingsParameter);
        }

        #endregion

        public async Task<IGitHubClientService> GetGitHubClient(IOperation operation)
        {
            var token = await portalSettingsStore.GetSettingsParameter(operation, PortalSettingsKey.GitHubToken);
            return !string.IsNullOrWhiteSpace(token) ? new GitHubClientService(token) : throw CommonExceptions.GitHubTokenIsNotStated(operation);
        }
    }
}