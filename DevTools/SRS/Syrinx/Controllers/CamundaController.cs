using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Containers.Camunda.Base;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.SRS.Camunda;
using VXDesign.Store.DevTools.SRS.Syrinx.Extensions;
using VXDesign.Store.DevTools.SRS.Syrinx.Models;
using CamundaRequestModel = VXDesign.Store.DevTools.SRS.Syrinx.Models.CamundaRequestModel;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamundaController : ApiController
    {
        private readonly ICamundaServerService camundaServerService;

        public CamundaController(ICamundaServerService camundaServerService)
        {
            this.camundaServerService = camundaServerService;
        }

        /// <summary>
        /// Obtains supported version of Camunda server
        /// </summary>
        /// <returns>String value of Camunda server version, e.g. "7.11"</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("version")]
        public string GetSupportedVersion() => "7.11";

        /// <summary>
        /// Obtains all supported APIs of Camunda server
        /// </summary>
        /// <returns>List of objects with collected endpoints information</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("info")]
        public IEnumerable<CamundaEndpointModel> GetSupportedApi() => CamundaEndpoint.GetAll().Select(CamundaEndpointModel.Transform);

        /// <summary>
        /// Sends some request to Camunda server
        /// </summary>
        /// <param name="model">Parameters to Camunda server</param>
        /// <returns>Response from Camunda server</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost("request")]
        public Task<ActionResult<CamundaResponseModel>> SendRequest([FromBody] CamundaRequestModel model) => HandleExceptionIfThrown(async () =>
        {
            var endpoint = CamundaEndpoint.GetEndpoint(model.Action) ?? throw CommonExceptions.CamundaEndpointIsNotFoundByActionCode();
            return (await camundaServerService.Send(model.ToEntity(endpoint))).ToModel();
        });
    }
}