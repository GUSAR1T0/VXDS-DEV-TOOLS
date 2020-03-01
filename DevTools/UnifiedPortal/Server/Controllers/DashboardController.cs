using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Core.Entities.Controllers;
using VXDesign.Store.DevTools.Core.Enums.Operations;
using VXDesign.Store.DevTools.Core.Services.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Dashboard;
using PortalPermission = VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication.PortalPermission;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Controllers
{
    [Route("api/[controller]")]
    public class DashboardController : BaseApiController
    {
        private readonly IDashboardService dashboardService;

        public DashboardController(IOperationService operationService, IDashboardService dashboardService) : base(operationService)
        {
            this.dashboardService = dashboardService;
        }

        /// <summary>
        /// Obtains data for admin panel
        /// </summary>
        /// <returns>Model of admin panel data</returns>
        [ProducesResponseType(typeof(DashboardModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet]
        public async Task<ActionResult<DashboardModel>> GetDashboardData() => await Execute(async operation =>
        {
            var entity = await dashboardService.GetDashboardData(operation);
            return entity.ToModel();
        });
    }
}