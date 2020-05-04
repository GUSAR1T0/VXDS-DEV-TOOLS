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
        /// Obtains information about a module
        /// </summary>
        /// <returns>Model of module data</returns>
        [ProducesResponseType(typeof(ModuleModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.AccessToAdminPanel)]
        [HttpGet("{moduleId}")]
        public async Task<ActionResult<ModuleModel>> GetModule(int moduleId) => await Execute(async operation =>
        {
            var module = await moduleService.GetModule(operation, moduleId);
            return module.ToModel();
        });

        /// <summary>
        /// Launches a module
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.ManageModules)]
        [HttpPut("{moduleId}/launch")]
        public async Task<ActionResult<ModuleModel>> LaunchModule(int moduleId) => await Execute(async operation => await moduleService.LaunchModule(operation, moduleId));

        /// <summary>
        /// Stops a module
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.ManageModules)]
        [HttpPut("{moduleId}/stop")]
        public async Task<ActionResult<ModuleModel>> StopModule(int moduleId) => await Execute(async operation => await moduleService.StopModule(operation, moduleId));

        /// <summary>
        /// Uploads module configuration
        /// </summary>
        /// <returns>Model of configuration data shortly</returns>
        [ProducesResponseType(typeof(List<ModuleConfigurationFileUploadResultModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [PortalAuthentication(PortalPermission.ManageModules)]
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

        /// <summary>
        /// Submits module configuration
        /// </summary>
        /// <returns>ID of module</returns>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.ManageModules)]
        [HttpPost("configuration")]
        public async Task<ActionResult<int>> SubmitModuleConfiguration([FromBody] ModuleConfigurationSubmitModel model) => await Execute(async operation =>
        {
            var moduleId = await moduleService.SubmitConfiguration(operation, model.ToEntity());
            return moduleId;
        });
    }
}