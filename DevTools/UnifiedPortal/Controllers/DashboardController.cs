using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Core.Attributes;
using VXDesign.Store.DevTools.Core.Entities.Controllers;
using VXDesign.Store.DevTools.Core.Services.Operations;
using VXDesign.Store.DevTools.Core.Services.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Models.Dashboard;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
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
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [SyrinxVerifiedAuthentication]
        [HttpGet("data")]
        public async Task<ActionResult<DashboardModel>> GetDashboardData() => await Execute(async operation =>
        {
            var entity = await dashboardService.GetDashboardData(operation);
            return entity.ToModel();
        });
    }
}