using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
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
        [ProducesResponseType(typeof(IAsyncEnumerable<HealthCheckModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet]
        public ActionResult<IAsyncEnumerable<HealthCheckModel>> GetHealthChecksData() => Execute(operation =>
        {
            var entities = healthChecksService.GetHealthChecksData(operation);
            return entities.ToModel();
        });
    }
}