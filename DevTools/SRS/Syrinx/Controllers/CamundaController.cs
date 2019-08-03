using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.SRS.Camunda;
using VXDesign.Store.DevTools.SRS.Camunda.Entities.API;
using VXDesign.Store.DevTools.SRS.Syrinx.Models.Camunda;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamundaController : ApiController
    {
        private readonly ICamundaService camundaService;

        public CamundaController(ICamundaService camundaService)
        {
            this.camundaService = camundaService;
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
        public ActionResult<CamundaResponseModel> SendRequest([FromBody] CamundaRequestModel model)
        {
            return HandleExceptionIfThrown<CamundaResponseModel>(() =>
            {
                var endpoint = CamundaEndpoint.GetEndpoint(model.Action) ?? throw new ApiControllerException("Failed to define endpoint by action code");
                return CamundaResponseModel.Transform(camundaService.Send(endpoint, model.Parameters).Result);
            });
        }
    }
}