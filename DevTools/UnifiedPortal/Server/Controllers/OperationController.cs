using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Services;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Incident;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Operation;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Controllers
{
    [Route("api/[controller]")]
    public class OperationController : BaseApiController
    {
        private readonly IOperationWithLogsService operationWithLogsService;

        public OperationController(IOperationService operationService, IOperationWithLogsService operationWithLogsService) : base(operationService)
        {
            this.operationWithLogsService = operationWithLogsService;
        }

        /// <summary>
        /// Obtains information about all events
        /// </summary>
        /// <returns>Model of operations with logs data</returns>
        [ProducesResponseType(typeof(OperationPagingResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpPost("list")]
        public async Task<ActionResult<OperationPagingResponseModel>> GetOperationsWithLogs([FromBody] OperationPagingRequestModel model) => await Execute(async operation =>
        {
            var items = await operationWithLogsService.GetItems(operation, model.ToEntity());
            var response = new OperationPagingResponseModel().ToModel(items);
            return response;
        });

        /// <summary>
        /// Obtains information about the incident
        /// </summary>
        /// <returns>Full information about the incident</returns>
        [ProducesResponseType(typeof(IncidentModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet("{id}/incident")]
        public async Task<ActionResult<IncidentModel>> GetIncident(long id) => await Execute(async operation =>
        {
            var entity = await operationWithLogsService.GetIncidentWithHistory(operation, id);
            return entity.ToModel();
        });

        /// <summary>
        /// Saves some comment for incident
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpPut("{id}/incident/comment")]
        public async Task<ActionResult> SaveIncidentComment(long id, [FromBody] IncidentCommentModel model) => await Execute(async operation =>
        {
            var portalPermissions = (PortalPermission) (UserPermissions.FirstOrDefault(item => item.PermissionGroupId == 1)?.Permissions ?? 0);
            if (model.HistoryId.HasValue &&
                UserId != await operationWithLogsService.GetChangeInitiator(operation, id, model.HistoryId.Value) &&
                (portalPermissions & PortalPermission.ManageIncidentComments) == 0)
            {
                throw CommonExceptions.AccessDenied(operation, StatusCodes.Status403Forbidden);
            }

            await operationWithLogsService.SaveComment(operation, id, UserId, model.HistoryId, model.Comment);
        });

        /// <summary>
        /// Removes the comment for incident
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpDelete("{id}/incident/comment/{historyId}")]
        public async Task<ActionResult> DeleteIncidentComment(long id, long historyId) => await Execute(async operation =>
        {
            var portalPermissions = (PortalPermission) (UserPermissions.FirstOrDefault(item => item.PermissionGroupId == 1)?.Permissions ?? 0);
            if (UserId != await operationWithLogsService.GetChangeInitiator(operation, id, historyId) &&
                (portalPermissions & PortalPermission.ManageIncidentComments) == 0)
            {
                throw CommonExceptions.AccessDenied(operation, StatusCodes.Status403Forbidden);
            }

            await operationWithLogsService.DeleteComment(operation, id, historyId);
        });

        /// <summary>
        /// Initializes incident from operation
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpPost("{id}/incident")]
        public async Task<ActionResult> InitializeIncident(long id, [FromBody] IncidentUpdateModel model) => await Execute(async operation =>
        {
            var entity = model.ToEntity(UserId);
            entity.IncidentOperationId = id;
            await operationWithLogsService.InitializeIncident(operation, entity);
        });

        /// <summary>
        /// Updates some incident
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpPut("{id}/incident")]
        public async Task<ActionResult> UpdateIncident(long id, [FromBody] IncidentUpdateModel model) => await Execute(async operation =>
        {
            var entity = model.ToEntity(UserId);
            entity.IncidentOperationId = id;
            await operationWithLogsService.UpdateIncident(operation, entity);
        });
    }
}