using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Enums.Operations;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.Common.Services.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Models.Dashboard;
using VXDesign.Store.DevTools.UnifiedPortal.Utils;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("api/[controller]")]
    public class DashboardController : ApiController
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
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [SyrinxVerifiedAuthentication(UserPermission.AccessToAdminPanel)]
        [HttpGet("data")]
        public async Task<ActionResult<DashboardModel>> GetAdminPanelData() => await Execute(OperationContexts.GetAdminPanelData, async operation =>
        {
            var entity = await dashboardService.GetDashboardData(operation);
            return entity.ToModel();
        });
    }
}