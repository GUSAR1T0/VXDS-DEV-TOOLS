using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Clients.Camunda;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Models.ProcessDefinition;
using VXDesign.Store.DevTools.Common.Clients.GitHub;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Models.Users;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost.Entities;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost.Extensions;
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
        Task<IEnumerable<HostSettingsEntity>> SearchHostsByPattern(IOperation operation, string pattern, IEnumerable<HostOperatingSystem> operatingSystems);
        Task AddHost(IOperation operation, HostSettingsItemEntity host);
        Task UpdateHost(IOperation operation, HostSettingsItemEntity host);
        Task DeleteHost(IOperation operation, int hostId);
        Task<int> GetAffectedModulesCount(IOperation operation, int hostId);
        Task<IDictionary<HostCredentialsItemEntity, CommandResult>> CheckConnections(IOperation operation, int hostId);
        (HostCredentialsItemEntity entity, CommandResult result) CheckConnection(IOperation operation, CheckConnectionToHostEntity entity);

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
        private readonly IModuleStore moduleStore;
        private readonly ISyrinxCamundaClientService camundaClient;

        public PortalSettingsService(IPortalSettingsStore portalSettingsStore, IModuleStore moduleStore, ISyrinxCamundaClientService camundaClient)
        {
            this.portalSettingsStore = portalSettingsStore;
            this.moduleStore = moduleStore;
            this.camundaClient = camundaClient;
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

        public async Task<IEnumerable<HostSettingsEntity>> SearchHostsByPattern(IOperation operation, string pattern, IEnumerable<HostOperatingSystem> operatingSystems)
        {
            return await portalSettingsStore.SearchHostsByPattern(operation, pattern, operatingSystems);
        }

        public async Task AddHost(IOperation operation, HostSettingsItemEntity host)
        {
            if (!await portalSettingsStore.IsHostNameUnique(operation, 0, host.Name))
            {
                throw CommonExceptions.HostNameIsNotUnique(operation);
            }

            await portalSettingsStore.AddHost(operation, host);
        }

        public async Task UpdateHost(IOperation operation, HostSettingsItemEntity host)
        {
            if (!await portalSettingsStore.IsHostExist(operation, host.Id))
            {
                throw CommonExceptions.HostWasNotFound(operation, host.Id);
            }

            if (!await portalSettingsStore.IsHostNameUnique(operation, host.Id, host.Name))
            {
                throw CommonExceptions.HostNameIsNotUnique(operation);
            }

            await portalSettingsStore.UpdateHost(operation, host);
        }

        public async Task DeleteHost(IOperation operation, int hostId)
        {
            if (!await portalSettingsStore.IsHostExist(operation, hostId))
            {
                throw CommonExceptions.HostWasNotFound(operation, hostId);
            }

            await portalSettingsStore.MakeHostInactive(operation, hostId);
            await new ProcessDefinition.StartProcessInstanceByKeyRequest(CamundaWorkerKey.HostDeletionProcess)
            {
                BusinessKey = hostId.ToString(),
                Variables = new CamundaVariables { { CamundaWorkerKey.HostId, hostId } }
            }.SendRequest(operation, camundaClient, true);
        }

        public async Task<int> GetAffectedModulesCount(IOperation operation, int hostId) => await moduleStore.GetHostModuleCount(operation, hostId);

        public async Task<IDictionary<HostCredentialsItemEntity, CommandResult>> CheckConnections(IOperation operation, int hostId)
        {
            if (!await portalSettingsStore.IsHostExist(operation, hostId))
            {
                throw CommonExceptions.HostWasNotFound(operation, hostId);
            }

            var host = await portalSettingsStore.GetHost(operation, hostId);
            return host.CredentialsList
                .Select(credentials =>
                {
                    var result = credentials.CheckConnection(host.OperatingSystem, host.Domain, out var client);
                    client.Dispose();
                    return new { credentials, result };
                })
                .Where(item => item.result != null)
                .ToDictionary(item => item.credentials, item => item.result);
        }

        public (HostCredentialsItemEntity entity, CommandResult result) CheckConnection(IOperation operation, CheckConnectionToHostEntity entity)
        {
            var credentials = new HostCredentialsItemEntity
            {
                Type = entity.Type,
                Port = entity.Port,
                Username = entity.Username,
                Password = entity.Password
            };
            var result = credentials.CheckConnection(entity.OperatingSystem, entity.Host, out var client);
            client.Dispose();
            return (credentials, result);
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