using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Core.Attributes;
using VXDesign.Store.DevTools.Core.Entities.Controllers;
using VXDesign.Store.DevTools.Core.Enums.Operations;
using VXDesign.Store.DevTools.Core.Services.Operations;
using VXDesign.Store.DevTools.Core.Services.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Models.Settings;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController : BaseApiController
    {
        private readonly IPortalSettingsService portalSettingsService;

        public SettingsController(IOperationService operationService, IPortalSettingsService portalSettingsService) : base(operationService)
        {
            this.portalSettingsService = portalSettingsService;
        }

        /// <summary>
        /// Obtains settings data
        /// </summary>
        /// <returns>Settings parameters</returns>
        [ProducesResponseType(typeof(SettingsParametersModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [SyrinxVerifiedAuthentication(PortalPermission.ManageSettings)]
        [HttpGet]
        public async Task<ActionResult<SettingsParametersModel>> GetSettings() => await Execute(async operation =>
        {
            var settings = await portalSettingsService.GetSettings(operation);
            return settings.ToModel();
        });

        #region GitHub

        /// <summary>
        /// Setup Personal Access Token of GitHub user
        /// </summary>
        [ProducesResponseType(typeof(GitHubUserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [SyrinxVerifiedAuthentication(PortalPermission.ManageSettings)]
        [HttpPut("codeService/github")]
        public async Task<ActionResult<GitHubUserModel>> SetupGitHubToken([FromQuery(Name = "t")] string token) => await Execute(async operation =>
        {
            var gitHubUser = await portalSettingsService.SetupGitHubToken(operation, token);
            return gitHubUser.ToModel();
        });

        #endregion
    }
}