using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Core.Attributes;
using VXDesign.Store.DevTools.Core.Entities.Controllers;
using VXDesign.Store.DevTools.Core.Enums.Operations;
using VXDesign.Store.DevTools.Core.Services.Operations;
using VXDesign.Store.DevTools.Core.Services.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("api/[controller]")]
    public class UserRoleController : BaseApiController
    {
        private readonly IUserRoleService userRoleService;
        private readonly IUserService userService;

        public UserRoleController(IOperationService operationService, IUserRoleService userRoleService, IUserService userService) : base(operationService)
        {
            this.userRoleService = userRoleService;
            this.userService = userService;
        }

        /// <summary>
        /// Obtains all user roles fully (with permissions)
        /// </summary>
        /// <returns>List of user roles</returns>
        [ProducesResponseType(typeof(IEnumerable<UserRoleFullInfoModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [SyrinxVerifiedAuthentication]
        [HttpGet("full/list")]
        public async Task<ActionResult<IEnumerable<UserRoleFullInfoModel>>> GetUserRolesFullInfo() => await Execute(async operation =>
        {
            var userRoles = await userRoleService.GetUserRoles(operation);
            return userRoles.Select(userRole => userRole.ToFullInfoModel());
        });

        /// <summary>
        /// Obtains all user roles shortly (without permissions)
        /// </summary>
        /// <returns>List of user roles</returns>
        [ProducesResponseType(typeof(IEnumerable<UserRoleShortInfoModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [SyrinxVerifiedAuthentication]
        [HttpGet("short/list")]
        public async Task<ActionResult<IEnumerable<UserRoleShortInfoModel>>> GetUserRolesShortInfo() => await Execute(async operation =>
        {
            var userRoles = await userRoleService.GetUserRoles(operation, false);
            return userRoles.Select(userRole => userRole.ToShortInfoModel());
        });

        /// <summary>
        /// Searches user roles by pattern
        /// </summary>
        /// <returns>List of user roles shortly (without permissions)</returns>
        [ProducesResponseType(typeof(IEnumerable<UserRoleShortInfoModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [SyrinxVerifiedAuthentication]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserRoleShortInfoModel>>> SearchUserRolesByPattern([FromQuery(Name = "p")] string pattern) => await Execute(async operation =>
        {
            var userRoles = await userRoleService.SearchUserRolesByPattern(operation, pattern);
            return userRoles.Select(userRole => userRole.ToShortInfoModel());
        });

        /// <summary>
        /// Obtains user role full (with permissions)
        /// </summary>
        /// <returns>List of user roles</returns>
        [ProducesResponseType(typeof(UserRoleFullInfoModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [SyrinxVerifiedAuthentication]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserRoleFullInfoModel>> GetUserRole(int id) => await Execute(async operation =>
        {
            var userRole = await userRoleService.GetUserRoleById(operation, id);
            return userRole.ToFullInfoModel();
        });

        /// <summary>
        /// Creates a new user role
        /// </summary>
        /// <param name="model">Model of user role for creation</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [SyrinxVerifiedAuthentication(PortalPermission.ManageUserRoles)]
        [HttpPost]
        public async Task<ActionResult> AddUserRole([FromBody] UserRoleFullInfoModel model)
        {
            return await Execute(async operation => await userRoleService.AddUserRole(operation, model.ToEntity()));
        }

        /// <summary>
        /// Updates an existed user role
        /// </summary>
        /// <param name="id">ID of an user role for update</param>
        /// <param name="model">Model of user role for update</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication(PortalPermission.ManageUserRoles)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserRole(int id, [FromBody] UserRoleFullInfoModel model)
        {
            return await Execute(async operation => await userRoleService.UpdateUserRole(operation, model.ToEntity(id)));
        }

        /// <summary>
        /// Removes an existed user role
        /// </summary>
        /// <param name="id">ID of an user role</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication(PortalPermission.ManageUserRoles)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserRole(int id) => await Execute(async operation => await userRoleService.DeleteUserRoleById(operation, id));

        /// <summary>
        /// Obtains a count of affected users before user role deletion
        /// </summary>
        /// <param name="id">ID of an user role</param>
        /// <returns>A count of affected users</returns>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [SyrinxVerifiedAuthentication]
        [HttpGet("{id}/affectedUsers/count")]
        public async Task<ActionResult<int>> GetAffectedUsersCount(int id) => await Execute(async operation =>
        {
            var count = await userService.GetAffectedUsersCount(operation, id);
            return count;
        });
    }
}