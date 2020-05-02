using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Services;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Module;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Controllers
{
    [Route("api/[controller]")]
    public class ModuleController : BaseApiController
    {
        private readonly IModuleService moduleService;

        public ModuleController(IOperationService operationService, IModuleService moduleService) : base(operationService)
        {
            this.moduleService = moduleService;
        }

        /// <summary>
        /// Obtains information about all modules
        /// </summary>
        /// <returns>Model of modules data</returns>
        [ProducesResponseType(typeof(ModulePagingResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpPost("list")]
        public async Task<ActionResult<ModulePagingResponseModel>> GetModules([FromBody] ModulePagingRequestModel model) => await Execute(async operation =>
        {
            var items = await moduleService.GetItems(operation, model.ToEntity());
            var response = new ModulePagingResponseModel().ToModel(items);
            return response;
        });

        /// <summary>
        /// Uploads module configuration
        /// </summary>
        /// <returns>Model of configuration data shortly</returns>
        [ProducesResponseType(typeof(List<ModuleConfigurationFileUploadResultModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        // [PortalAuthentication(PortalPermission.ManageModules)]
        [HttpPost("configuration/upload")]
        public async Task<ActionResult<List<ModuleConfigurationFileUploadResultModel>>> UploadModuleConfiguration(List<IFormFile> files) => await Execute(async operation =>
        {
            var results = new List<ModuleConfigurationFileUploadResultModel>();
            if (!files.IsNullOrEmpty())
            {
                foreach (var file in files)
                {
                    var result = await moduleService.ReadConfiguration(operation, file.ToEntity());
                    results.Add(result.ToModel());
                }
            }

            return results;
        });
    }
}