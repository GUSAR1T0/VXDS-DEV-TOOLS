using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Enums.Operations;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.Common.Services.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;
using VXDesign.Store.DevTools.UnifiedPortal.Utils;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("api/[controller]")]
    public class UserRoleController : ApiController
    {
        private readonly IUserRoleService userRoleService;

        public UserRoleController(IOperationService operationService, IUserRoleService userRoleService) : base(operationService)
        {
            this.userRoleService = userRoleService;
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
        public async Task<ActionResult<IEnumerable<UserRoleFullInfoModel>>> GetUserRolesFullInfo() => await Execute(OperationContexts.GetUserRolesFullInfo, async operation =>
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
        public async Task<ActionResult<IEnumerable<UserRoleShortInfoModel>>> GetUserRolesShortInfo() => await Execute(OperationContexts.GetUserRolesShortInfo, async operation =>
        {
            var userRoles = await userRoleService.GetUserRoles(operation, false);
            return userRoles.Select(userRole => userRole.ToShortInfoModel());
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
        [SyrinxVerifiedAuthentication(userRolePermissions: UserRolePermission.CreateRole)]
        [HttpPost]
        public async Task<ActionResult> AddUserRole([FromBody] UserRoleFullInfoModel model)
        {
            return await Execute(OperationContexts.AddUserRole, async operation => await userRoleService.AddUserRole(operation, model.ToEntity()));
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
        [SyrinxVerifiedAuthentication(userRolePermissions: UserRolePermission.UpdateRole)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserRole(int id, [FromBody] UserRoleFullInfoModel model)
        {
            return await Execute(OperationContexts.UpdateUserRole, async operation => await userRoleService.UpdateUserRole(operation, model.ToEntity(id)));
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
        [SyrinxVerifiedAuthentication(userRolePermissions: UserRolePermission.DeleteRole)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserRole(int id) => await Execute(OperationContexts.DeleteUserRole, async operation => await userRoleService.DeleteUserRoleById(operation, id));
    }
}