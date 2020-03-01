using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Core.Entities.Controllers;
using VXDesign.Store.DevTools.Core.Enums.Operations;
using VXDesign.Store.DevTools.Core.Services.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Operation;
using PortalPermission = VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication.PortalPermission;

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
    }
}