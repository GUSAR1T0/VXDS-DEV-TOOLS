using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.SRS.Camunda;
using VXDesign.Store.DevTools.SRS.Syrinx.Extensions;
using VXDesign.Store.DevTools.SRS.Syrinx.Models.Camunda;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamundaController : BaseApiController
    {
        private readonly ICamundaServerService camundaServerService;

        public CamundaController(IOperationService operationService, ICamundaServerService camundaServerService) : base(operationService)
        {
            this.camundaServerService = camundaServerService;
        }

        /// <summary>
        /// Obtains supported version of Camunda server
        /// </summary>
        /// <returns>String value of Camunda server version, e.g. "7.11"</returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("version")]
        public async Task<ActionResult<string>> GetSupportedVersion() => await Execute(async operation =>
        {
            var response = await camundaServerService.Send(operation, new CamundaRequest
            {
                Endpoint = CamundaEndpoint.GetEndpoint(CamundaAction.Version),
                Path = new Dictionary<string, string>(),
                Query = new Dictionary<string, string>(),
                Body = "{}",
                Resources = new List<LocalFile>()
            });

            if (response.IsWithoutErrors())
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Output)["version"];
            }

            throw CommonExceptions.FailedToGetCamundaVersion(operation);
        });

        /// <summary>
        /// Obtains all supported APIs of Camunda server
        /// </summary>
        /// <returns>List of objects with collected endpoints information</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("info")]
        public IEnumerable<CamundaEndpointModel> GetSupportedApi() => CamundaEndpoint.GetAll().Select(CamundaEndpointModel.Transform);

        /// <summary>
        /// Sends some request to Camunda server
        /// </summary>
        /// <param name="model">Parameters to Camunda server</param>
        /// <returns>Response from Camunda server</returns>
        [ProducesResponseType(typeof(CamundaResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        [HttpPost("request")]
        public async Task<ActionResult<CamundaResponseModel>> SendRequest([FromForm] CamundaRequestModel model) => await Execute(async operation =>
        {
            var endpoint = CamundaEndpoint.GetEndpoint(model.Action) ?? throw CommonExceptions.CamundaEndpointIsNotFoundByActionCode(operation);
            return (await camundaServerService.Send(operation, model.ToEntity(endpoint))).ToModel();
        });
    }
}