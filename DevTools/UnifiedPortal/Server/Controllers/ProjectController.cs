using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Services;
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
        public async Task<ActionResult<ProjectPagingResponseModel>> GetProjects([FromBody] ProjectPagingRequestModel model) => await Execute(async operation =>
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

        /// <summary>
        /// Obtains information about the project
        /// </summary>
        /// <returns>Full information about the project</returns>
        [ProducesResponseType(typeof(ProjectProfileGetModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectProfileGetModel>> GetProjectProfile(int id) => await Execute(async operation =>
        {
            var entity = await projectService.GetProjectProfileById(operation, id);
            return entity.ToModel();
        });

        /// <summary>
        /// Creates new project
        /// </summary>
        /// <returns>ID of new project</returns>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [PortalAuthentication(PortalPermission.ManageProjects)]
        [HttpPost]
        public async Task<ActionResult<int>> CreateProjectProfile([FromBody] ProjectProfileModel model) => await Execute(async operation =>
        {
            var id = await projectService.CreateProjectProfile(operation, model.ToEntity());
            return id;
        });

        /// <summary>
        /// Updates some project
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.ManageProjects)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProjectProfile(int id, [FromBody] ProjectProfileModel model) => await Execute(async operation =>
        {
            var entity = model.ToEntity();
            entity.Id = id;
            await projectService.UpdateProjectProfile(operation, entity);
        });

        /// <summary>
        /// Removes some project
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication(PortalPermission.ManageProjects)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProjectProfile(int id) => await Execute(async operation => await projectService.DeleteProjectProfile(operation, id));
    }
}