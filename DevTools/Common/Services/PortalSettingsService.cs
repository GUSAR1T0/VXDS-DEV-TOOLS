using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Clients.GitHub;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Models.Users;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost.Entities;
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

        public PortalSettingsService(IPortalSettingsStore portalSettingsStore, IModuleStore moduleStore)
        {
            this.portalSettingsStore = portalSettingsStore;
            this.moduleStore = moduleStore;
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
            if (!await portalSettingsStore.IsHostExist(operation, host.Id))
            {
                throw CommonExceptions.HostWasNotFound(operation, host.Id);
            }

            if (!await portalSettingsStore.IsHostNameUnique(operation, 0, host.Name))
            {
                throw CommonExceptions.HostNameIsNotUnique(operation);
            }

            await portalSettingsStore.AddHost(operation, host);
        }

        public async Task UpdateHost(IOperation operation, HostSettingsItemEntity host)
        {
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

            // TODO: Trigger to delete modules before
            await portalSettingsStore.DeleteHost(operation, hostId);
        }

        public async Task<int> GetAffectedModulesCount(IOperation operation, int hostId) => await moduleStore.GetModuleCount(operation, hostId);

        public async Task<IDictionary<HostCredentialsItemEntity, CommandResult>> CheckConnections(IOperation operation, int hostId)
        {
            if (!await portalSettingsStore.IsHostExist(operation, hostId))
            {
                throw CommonExceptions.HostWasNotFound(operation, hostId);
            }

            var host = await portalSettingsStore.GetHost(operation, hostId);
            return host.CredentialsList
                .Select(credentials => new
                {
                    credentials,
                    check = CheckConnection(host.OperatingSystem, host.Domain, credentials)
                })
                .Where(item => item.check != null)
                .ToDictionary(item => item.credentials, item => item.check);
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
            return (credentials, CheckConnection(entity.OperatingSystem, entity.Host, credentials));
        }

        private static CommandResult CheckConnection(HostOperatingSystem operatingSystem, string host, HostCredentialsItemEntity credentials)
        {
            string command;
            var arguments = new List<string>();
            switch (operatingSystem)
            {
                case HostOperatingSystem.MacOS:
                case HostOperatingSystem.Linux:
                    command = "uname";
                    arguments.Add("-a");
                    break;
                case HostOperatingSystem.WindowsOS:
                    command = "ver";
                    break;
                default:
                    command = "";
                    break;
            }

            if (credentials.Type == HostConnectionType.SSH)
            {
                try
                {
                    using var sshClient = SshClientService.Create(new SshConnectionByPasswordCredentials
                    {
                        Host = host,
                        Port = credentials.Port,
                        Username = credentials.Username,
                        Password = credentials.Password
                    });
                    return sshClient.Send(command, arguments.ToArray());
                }
                catch (Exception e)
                {
                    return new CommandResult
                    {
                        Command = $"{command} {string.Join(' ', arguments)}",
                        Output = null,
                        Error = e.Message,
                        ExitStatus = -1
                    };
                }
            }

            return null;
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