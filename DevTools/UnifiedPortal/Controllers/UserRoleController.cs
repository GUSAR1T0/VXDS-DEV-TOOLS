using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Services.AST;
using VXDesign.Store.DevTools.UnifiedPortal.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("api/[controller]")]
    public class UserRoleController : ApiController
    {
        private readonly IUserRoleService userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            this.userRoleService = userRoleService;
        }

        /// <summary>
        /// Obtains all user roles
        /// </summary>
        /// <returns>List of user roles</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SyrinxVerifiedAuthentication]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<UserRoleModel>>> GetUserRoles() => await HandleExceptionIfThrown(async () =>
        {
            var userRoles = await userRoleService.GetUserRoles();
            return userRoles.Select(userRole => userRole.ToModel());
        });

        /// <summary>
        /// Creates a new user role
        /// </summary>
        /// <param name="model">Model of user role for creation</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SyrinxVerifiedAuthentication]
        [HttpPost]
        public async Task AddUserRole([FromBody] UserRoleModel model) => await HandleExceptionIfThrown(async () => await userRoleService.AddUserRole(model.ToEntity()));

        /// <summary>
        /// Updates an existed user role
        /// </summary>
        /// <param name="id">ID of an user role for update</param>
        /// <param name="model">Model of user role for update</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpPut("{id}")]
        public async Task UpdateUserRole(string id, [FromBody] UserRoleModel model) => await HandleExceptionIfThrown(async () => await userRoleService.UpdateUserRole(model.ToEntity(id)));

        /// <summary>
        /// Removes an existed user role
        /// </summary>
        /// <param name="id">ID of an user role</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpDelete("{id}")]
        public async Task DeleteUserRole(string id) => await HandleExceptionIfThrown(async () => await userRoleService.DeleteUserRoleById(id));
    }
}