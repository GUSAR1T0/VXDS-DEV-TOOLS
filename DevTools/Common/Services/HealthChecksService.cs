using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Clients.Camunda;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Models.Version;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.HealthCheck;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface IHealthChecksService
    {
        Task<IEnumerable<HealthCheckEntity>> GetHealthChecksData(IOperation operation);
    }

    public class HealthChecksService : IHealthChecksService
    {
        private readonly ISyrinxCamundaClientService camundaService;
        private readonly IMemoryCacheService memoryCacheService;
        private readonly IPortalSettingsStore portalSettingsStore;
        private readonly IPortalSettingsService portalSettingsService;
        private readonly IModuleStore moduleStore;

        public HealthChecksService(ISyrinxCamundaClientService camundaService, IMemoryCacheService memoryCacheService, IPortalSettingsStore portalSettingsStore,
            IPortalSettingsService portalSettingsService, IModuleStore moduleStore)
        {
            this.camundaService = camundaService;
            this.memoryCacheService = memoryCacheService;
            this.portalSettingsStore = portalSettingsStore;
            this.portalSettingsService = portalSettingsService;
            this.moduleStore = moduleStore;
        }

        public async Task<IEnumerable<HealthCheckEntity>> GetHealthChecksData(IOperation operation)
        {
            var hosts = await portalSettingsStore.GetHosts(operation);
            var modules = await moduleStore.GetModules(operation);

            var all = await Task.WhenAll(
                CheckCamunda(operation),
                Task.Run(() => CheckHosts(operation, hosts)),
                Task.Run(() => CheckModules(modules))
            );
            return all.SelectMany(x => x);
        }

        private async Task<IEnumerable<HealthCheckEntity>> CheckCamunda(IOperation operation)
        {
            return new[]
            {
                new HealthCheckEntity
                {
                    Type = "Camunda",
                    IsOk = await GetCamundaHealthChecksData(operation)
                }
            };
        }

        private IEnumerable<HealthCheckEntity> CheckHosts(IOperation operation, IEnumerable<HostSettingsItemEntity> hosts)
        {
            var checks = new List<HealthCheckEntity>();
            foreach (var host in hosts)
            {
                var hostCredentials = host.CredentialsList.ToList();
                for (var i = 0; i < hostCredentials.Count; i++)
                {
                    var (_, result) = portalSettingsService.CheckConnection(operation, new CheckConnectionToHostEntity
                    {
                        OperatingSystem = host.OperatingSystem,
                        Host = host.Domain,
                        Type = hostCredentials[i].Type,
                        Port = hostCredentials[i].Port,
                        Username = hostCredentials[i].Username,
                        Password = hostCredentials[i].Password
                    });

                    var credentialsInfo = host.Domain;

                    if (!string.IsNullOrWhiteSpace(hostCredentials[i].Username))
                    {
                        credentialsInfo = $"{hostCredentials[i].Username}@{credentialsInfo}";
                    }

                    if (hostCredentials[i].Port.HasValue)
                    {
                        credentialsInfo = $"{credentialsInfo}:{hostCredentials[i].Port}";
                    }

                    checks.Add(new HealthCheckEntity
                    {
                        Type = $"Host \"{host.Name}\" (credentials #{i + 1}, {credentialsInfo})",
                        IsOk = !result.HasErrors
                    });
                }
            }

            return checks;
        }

        private IEnumerable<HealthCheckEntity> CheckModules(IEnumerable<ModuleShortEntity> modules)
        {
            var checks = new List<HealthCheckEntity>();
            checks.AddRange(modules.Select(module => new HealthCheckEntity
            {
                Type = $"Module \"{module.Name}\" ({module.Alias}, ver.{module.Version})",
                IsOk = module.Status != ModuleStatus.FailedToInstall || module.Status != ModuleStatus.FailedToUpgrade || module.Status != ModuleStatus.FailedToDowngrade ||
                       module.Status != ModuleStatus.FailedToUninstall || module.Status != ModuleStatus.FailedToRun || module.Status != ModuleStatus.FailedToStop
            }));

            return checks;
        }

        private async Task<bool> GetCamundaHealthChecksData(IOperation operation)
        {
            try
            {
                var camundaVersion = await GetCamundaVersionFromCache(operation);
                return camundaVersion.IsWithoutErrors();
            }
            catch
            {
                return false;
            }
        }

        private async Task<Version.GetVersionResponse> GetCamundaVersionFromCache(IOperation operation)
        {
            return await memoryCacheService.Get(MemoryCacheKey.CamundaVersion, async () => await new Version.GetVersionRequest().SendRequest(operation, camundaService));
        }
    }
}