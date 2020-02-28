using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Core.Entities.Controllers;
using VXDesign.Store.DevTools.Core.Services.Operations;
using VXDesign.Store.DevTools.Core.Services.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.GitHub;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Project;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : BaseApiController
    {
        private readonly IProjectService projectService;

        public ProjectController(IOperationService operationService, IProjectService projectService) : base(operationService)
        {
            this.projectService = projectService;
        }

        /// <summary>
        /// Obtains information about all projects
        /// </summary>
        /// <returns>Model of projects with repositories data</returns>
        [ProducesResponseType(typeof(ProjectPagingResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [PortalAuthentication]
        [HttpPost("list")]
        public async Task<ActionResult<ProjectPagingResponseModel>> Get([FromBody] ProjectPagingRequestModel model) => await Execute(async operation =>
        {
            var items = await projectService.GetItems(operation, model.ToEntity());
            var response = new ProjectPagingResponseModel().ToModel(items);
            return response;
        });

        /// <summary>
        /// Searches GitHub repositories by pattern
        /// </summary>
        /// <returns>List of GitHub repositories shortly</returns>
        [ProducesResponseType(typeof(IEnumerable<GitHubRepositoryShortModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [PortalAuthentication]
        [HttpGet("github/search")]
        public async Task<ActionResult<IEnumerable<GitHubRepositoryShortModel>>> SearchGitHubRepositoriesByPattern([FromQuery(Name = "p")] string pattern) => await Execute(async operation =>
        {
            var repositories = await projectService.SearchGitHubRepositoriesByPattern(operation, pattern);
            return repositories.Select(repository => repository.ToModel());
        });
    }
}