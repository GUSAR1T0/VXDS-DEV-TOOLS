using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Services;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.GitHub;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Settings;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController : BaseApiController
    {
        private readonly IPortalSettingsService portalSettingsService;

        public SettingsController(IOperationService operationService, IPortalSettingsService portalSettingsService) : base(operationService)
        {
            this.portalSettingsService = portalSettingsService;
        }

        #region Hosts

        /// <summary>
        /// Obtains hosts data
        /// </summary>
        /// <returns>Settings parameters</returns>
        [ProducesResponseType(typeof(HostPagingResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.ManageSettings)]
        [HttpPost("host/list")]
        public async Task<ActionResult<HostPagingResponseModel>> GetHosts([FromBody] HostPagingRequestModel model) => await Execute(async operation =>
        {
            var hosts = await portalSettingsService.GetHosts(operation, model.ToEntity());
            return new HostPagingResponseModel().ToModel(hosts);
        });

        /// <summary>
        /// Searches hosts by pattern
        /// </summary>
        /// <returns>List of hosts shortly</returns>
        [ProducesResponseType(typeof(IEnumerable<HostSettingsShortModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet("host/search")]
        public async Task<ActionResult<IEnumerable<HostSettingsShortModel>>> SearchUsersByPattern([FromQuery(Name = "p")] string pattern,
            [FromQuery(Name = "os")] IEnumerable<HostOperatingSystem> operatingSystems = null)
        {
            return await Execute(async operation =>
            {
                var hosts = await portalSettingsService.SearchHostsByPattern(operation, pattern, operatingSystems ?? new List<HostOperatingSystem>());
                return hosts.Select(host => host.ToModel());
            });
        }

        /// <summary>
        /// Adds new host data
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.ManageSettings)]
        [HttpPost("host")]
        public async Task<ActionResult> AddHost([FromBody] HostSettingsItemModel model) => await Execute(async operation =>
        {
            var entity = model.ToEntity();
            await portalSettingsService.AddHost(operation, entity);
        });

        /// <summary>
        /// Updates an existed host data
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.ManageSettings)]
        [HttpPut("host/{id}")]
        public async Task<ActionResult> UpdateHost(int id, [FromBody] HostSettingsItemModel model) => await Execute(async operation =>
        {
            var entity = model.ToEntity(id);
            await portalSettingsService.UpdateHost(operation, entity);
        });

        /// <summary>
        /// Removes an existed host data
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.ManageSettings)]
        [HttpDelete("host/{id}")]
        public async Task<ActionResult> DeleteHost(int id) => await Execute(async operation => await portalSettingsService.DeleteHost(operation, id));

        /// <summary>
        /// Obtains a count of affected modules before host deletion
        /// </summary>
        /// <param name="id">ID of an host</param>
        /// <returns>A count of affected modules</returns>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.ManageSettings)]
        [HttpGet("host/{id}/affected/count")]
        public async Task<ActionResult<int>> GetAffectedModulesCount(int id) => await Execute(async operation =>
        {
            var count = await portalSettingsService.GetAffectedModulesCount(operation, id);
            return count;
        });

        /// <summary>
        /// Checks all host connections
        /// </summary>
        /// <param name="id">ID of an host</param>
        /// <returns>Dictionary of host credentials and command result of check</returns>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.ManageSettings)]
        [HttpGet("host/{id}/check")]
        public async Task<ActionResult<IEnumerable<CheckConnectionsToHostResultModel>>> CheckConnections(int id) => await Execute(async operation =>
        {
            var checks = await portalSettingsService.CheckConnections(operation, id);
            return checks.ToModels();
        });

        /// <summary>
        /// Check some host connection
        /// </summary>
        /// <returns>Host credentials and command result of check</returns>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.ManageSettings)]
        [HttpPost("host/check")]
        public ActionResult<CheckConnectionsToHostResultModel> CheckConnection([FromBody] CheckConnectionToHostModel model) => Execute(operation =>
        {
            var checks = portalSettingsService.CheckConnection(operation, model.ToEntity());
            return checks.ToModel();
        });

        #endregion

        #region Code Services

        /// <summary>
        /// Obtains code services settings data
        /// </summary>
        /// <returns>Settings parameters</returns>
        [ProducesResponseType(typeof(CodeServicesSettingsModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.ManageSettings)]
        [HttpGet("codeService")]
        public async Task<ActionResult<CodeServicesSettingsModel>> GetCodeServicesData() => await Execute(async operation =>
        {
            var settings = await portalSettingsService.GetCodeServicesData(operation);
            return settings.ToModel();
        });

        /// <summary>
        /// Setup Personal Access Token of GitHub user
        /// </summary>
        [ProducesResponseType(typeof(GitHubUserProfileModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.ManageSettings)]
        [HttpPut("codeService/github/token")]
        public async Task<ActionResult<GitHubUserProfileModel>> SetupGitHubToken([FromQuery(Name = "t")] string token) => await Execute(async operation =>
        {
            var gitHubUser = await portalSettingsService.SetupGitHubToken(operation, token);
            return gitHubUser.ToModel();
        });

        #endregion
    }
}