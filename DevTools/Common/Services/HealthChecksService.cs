using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Clients.Camunda;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Models.Version;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.HealthCheck;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface IHealthChecksService
    {
        IAsyncEnumerable<HealthCheckEntity> GetHealthChecksData(IOperation operation);
    }

    public class HealthChecksService : IHealthChecksService
    {
        private readonly ISyrinxCamundaClientService camundaService;
        private readonly IMemoryCacheService memoryCacheService;

        public HealthChecksService(ISyrinxCamundaClientService camundaService, IMemoryCacheService memoryCacheService)
        {
            this.camundaService = camundaService;
            this.memoryCacheService = memoryCacheService;
        }

        public async IAsyncEnumerable<HealthCheckEntity> GetHealthChecksData(IOperation operation)
        {
            yield return new HealthCheckEntity
            {
                Type = "Camunda",
                IsOk = await GetCamundaHealthChecksData(operation)
            };
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