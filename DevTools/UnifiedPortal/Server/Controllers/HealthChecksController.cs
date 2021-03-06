using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Services;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.HealthCheck;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Controllers
{
    [Route("api/[controller]")]
    public class HealthChecksController : BaseApiController
    {
        private readonly IHealthChecksService healthChecksService;

        public HealthChecksController(IOperationService operationService, IHealthChecksService healthChecksService) : base(operationService)
        {
            this.healthChecksService = healthChecksService;
        }

        /// <summary>
        /// Obtains health checks data
        /// </summary>
        /// <returns>Model of health checks data</returns>
        [ProducesResponseType(typeof(IEnumerable<HealthCheckModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthCheckModel>>> GetHealthChecksData() => await Execute(async operation =>
        {
            var entities = await healthChecksService.GetHealthChecksData(operation);
            return entities.ToModel();
        });
    }
}