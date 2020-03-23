using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Services;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Incident;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Controllers
{
    [Route("api/[controller]")]
    public class IncidentController : BaseApiController
    {
        private readonly IIncidentService incidentService;

        public IncidentController(IOperationService operationService, IIncidentService incidentService) : base(operationService)
        {
            this.incidentService = incidentService;
        }

        /// <summary>
        /// Obtains information about all incidents
        /// </summary>
        /// <returns>Model of incidents data</returns>
        [ProducesResponseType(typeof(IncidentPagingResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.ManageIncidents)]
        [HttpPost("list")]
        public async Task<ActionResult<IncidentPagingResponseModel>> GetIncidents([FromBody] IncidentPagingRequestModel model) => await Execute(async operation =>
        {
            var response = await incidentService.GetItems(operation, model.ToEntity());
            return new IncidentPagingResponseModel().ToModel(response);
        });

        /// <summary>
        /// Obtains information about the incident
        /// </summary>
        /// <returns>Full information about the incident</returns>
        [ProducesResponseType(typeof(IncidentGetModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.ManageIncidents)]
        [HttpGet("{operationId}")]
        public async Task<ActionResult<IncidentGetModel>> GetIncident(int operationId) => await Execute(async operation =>
        {
            var entity = await incidentService.GetIncidentWithHistory(operation, operationId);
            return entity.ToModel();
        });

        /// <summary>
        /// Initializes incident from operation
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [PortalAuthentication(PortalPermission.ManageIncidents)]
        [HttpPost]
        public async Task<ActionResult> InitializeIncident([FromBody] IncidentUpdateModel model)
        {
            return await Execute(async operation => await incidentService.InitializeIncident(operation, model.ToEntity()));
        }

        /// <summary>
        /// Updates some incident
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.ManageIncidents)]
        [HttpPut("{operationId}")]
        public async Task<ActionResult> UpdateProjectProfile(int operationId, [FromBody] IncidentUpdateModel model) => await Execute(async operation =>
        {
            var entity = model.ToEntity();
            entity.OperationId = operationId;
            await incidentService.UpdateIncident(operation, entity);
        });
    }
}