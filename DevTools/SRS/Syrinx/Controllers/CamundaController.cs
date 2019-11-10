﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Core.Entities.Camunda.Base;
using VXDesign.Store.DevTools.Core.Entities.Controllers;
using VXDesign.Store.DevTools.Core.Entities.Exceptions;
using VXDesign.Store.DevTools.Core.Services.Operations;
using VXDesign.Store.DevTools.SRS.Camunda;
using VXDesign.Store.DevTools.SRS.Syrinx.Extensions;
using VXDesign.Store.DevTools.SRS.Syrinx.Models.Camunda;
using VXDesign.Store.DevTools.SRS.Syrinx.Utils;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("version")]
        public string GetSupportedVersion() => "7.11";

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
        public async Task<ActionResult<CamundaResponseModel>> SendRequest([FromBody] CamundaRequestModel model) => await Execute(OperationContexts.SendRequest, async operation =>
        {
            var endpoint = CamundaEndpoint.GetEndpoint(model.Action) ?? throw CommonExceptions.CamundaEndpointIsNotFoundByActionCode(operation);
            return (await camundaServerService.Send(operation, model.ToEntity(endpoint))).ToModel();
        });
    }
}