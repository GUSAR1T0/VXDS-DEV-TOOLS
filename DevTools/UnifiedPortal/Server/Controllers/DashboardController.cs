using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Services;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Dashboard;

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
        /// Obtains notifications data for admin panel
        /// </summary>
        /// <returns>Model of admin panel data</returns>
        [ProducesResponseType(typeof(NotificationsDataModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet("notifications")]
        public async Task<ActionResult<NotificationsDataModel>> GetNotificationsData() => await Execute(async operation =>
        {
            var entity = await dashboardService.GetNotificationsData(operation);
            return entity.ToModel();
        });

        /// <summary>
        /// Obtains incidents data for admin panel
        /// </summary>
        /// <returns>Model of admin panel data</returns>
        [ProducesResponseType(typeof(IncidentsDataModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet("incidents")]
        public async Task<ActionResult<IncidentsDataModel>> GetIncidentsData() => await Execute(async operation =>
        {
            var entity = await dashboardService.GetIncidentsData(operation, UserId.Value);
            return entity.ToModel();
        });

        /// <summary>
        /// Obtains server date / time data for admin panel
        /// </summary>
        /// <returns>Model of admin panel data</returns>
        [ProducesResponseType(typeof(ServerDateTimeModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet("serverDateTime")]
        public ActionResult<ServerDateTimeModel> GetServerDateTime() => Execute(_ =>
        {
            var dateTime = DateTime.Now.ToUniversalTime();
            return new ServerDateTimeModel
            {
                Hours = dateTime.ToString("HH"),
                Minutes = dateTime.ToString("mm"),
                DayOfWeek = dateTime.ToString("dddd"),
                Date = dateTime.ToString("dd MMMM yyyy")
            };
        });

        /// <summary>
        /// Obtains system health check data for admin panel
        /// </summary>
        /// <returns>Model of admin panel data</returns>
        [ProducesResponseType(typeof(SystemHealthCheckDataModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet("systemHealthCheck")]
        public async Task<ActionResult<SystemHealthCheckDataModel>> GetSystemHealthCheck() => await Execute(async operation => new SystemHealthCheckDataModel
        {
            IsOk = await dashboardService.IsSystemHealthStatusOk(operation)
        });

        /// <summary>
        /// Obtains users data for admin panel
        /// </summary>
        /// <returns>Model of admin panel data</returns>
        [ProducesResponseType(typeof(UsersDataModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet("users")]
        public async Task<ActionResult<UsersDataModel>> GetUsersData() => await Execute(async operation =>
        {
            var entity = await dashboardService.GetUsersData(operation);
            return entity.ToModel();
        });

        /// <summary>
        /// Obtains user roles data for admin panel
        /// </summary>
        /// <returns>Model of admin panel data</returns>
        [ProducesResponseType(typeof(UserRolesDataModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet("userRoles")]
        public async Task<ActionResult<UserRolesDataModel>> GetUserRolesData() => await Execute(async operation =>
        {
            var entity = await dashboardService.GetUserRolesData(operation);
            return entity.ToModel();
        });

        /// <summary>
        /// Obtains projects data for admin panel
        /// </summary>
        /// <returns>Model of admin panel data</returns>
        [ProducesResponseType(typeof(ProjectsDataModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet("projects")]
        public async Task<ActionResult<ProjectsDataModel>> GetProjectsData() => await Execute(async operation =>
        {
            var entity = await dashboardService.GetProjectsData(operation);
            return entity.ToModel();
        });

        /// <summary>
        /// Obtains system statistics data for admin panel
        /// </summary>
        /// <returns>Model of admin panel data</returns>
        [ProducesResponseType(typeof(SystemStatisticsDataModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet("system")]
        public async Task<ActionResult<SystemStatisticsDataModel>> GetSystemStatisticsData() => await Execute(async operation =>
        {
            var entity = await dashboardService.GetSystemStatisticsData(operation);
            return entity.ToModel();
        });
    }
}