using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Core.Attributes;
using VXDesign.Store.DevTools.Core.Entities.Controllers;
using VXDesign.Store.DevTools.Core.Entities.Exceptions;
using VXDesign.Store.DevTools.Core.Enums.Operations;
using VXDesign.Store.DevTools.Core.Services.Operations;
using VXDesign.Store.DevTools.Core.Services.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;
using VXDesign.Store.DevTools.UnifiedPortal.Utils;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseApiController
    {
        private readonly IUserService userService;

        public UserController(IOperationService operationService, IUserService userService) : base(operationService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Obtains all users info
        /// </summary>
        /// <returns>List of users with their info</returns>
        [ProducesResponseType(typeof(IEnumerable<UserModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [SyrinxVerifiedAuthentication]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers() => await Execute(OperationContexts.GetUsers, async operation =>
        {
            var users = await userService.GetUsers(operation);
            return users.Select(user => user.ToModel());
        });

        /// <summary>
        /// Obtains user profile data by ID
        /// </summary>
        /// <param name="id">Unique user ID</param>
        /// <returns>User data if it was found</returns>
        [ProducesResponseType(typeof(UserProfileGetModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileGetModel>> GetUserProfile(int id) => await Execute(OperationContexts.GetUserProfile, async operation =>
        {
            var entity = await userService.GetUserProfileById(operation, id);
            return entity.ToModel();
        });

        /// <summary>
        /// Updates user profile general info data
        /// </summary>
        /// <param name="id">Unique user ID for update</param>
        /// <param name="model">Model of user profile general info for update</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpPut("{id}/general")]
        public async Task<ActionResult> UpdateUserProfileGeneralInfo(int id, [FromBody] UserProfileGeneralInfoUpdateModel model)
        {
            return await Execute(OperationContexts.UpdateUserProfileGeneralInfo, async operation =>
            {
                if (UserId != id && (UserPermissions & UserPermission.UpdateUserProfile) == 0)
                {
                    throw CommonExceptions.AccessDenied(operation, StatusCodes.Status403Forbidden);
                }

                var entity = model.ToEntity(id);
                await userService.UpdateUserProfileGeneralInfo(operation, entity);
            });
        }

        /// <summary>
        /// Updates user profile account specific info data
        /// </summary>
        /// <param name="id">Unique user ID for update</param>
        /// <param name="model">Model of user profile account specific info for update</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpPut("{id}/accountSpecific")]
        public async Task<ActionResult> UpdateUserProfileAccountSpecificInfo(int id, [FromBody] UserProfileAccountSpecificInfoUpdateModel model)
        {
            return await Execute(OperationContexts.UpdateUserProfileAccountSpecificInfo, async operation =>
            {
                if (UserId != id && (UserPermissions & UserPermission.UpdateUserProfile) == 0)
                {
                    throw CommonExceptions.AccessDenied(operation, StatusCodes.Status403Forbidden);
                }

                var entity = model.ToEntity(id);
                await userService.UpdateUserProfileAccountSpecificInfo(operation, entity);
            });
        }

        /// <summary>
        /// Activates user account
        /// </summary>
        /// <param name="id">Unique user ID</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpPut("{id}/activate")]
        public async Task<ActionResult> ActivateUser(int id) => await Execute(OperationContexts.ActivateUser, async operation =>
        {
            if (UserId != id && (UserPermissions & UserPermission.UpdateUserProfile) == 0)
            {
                throw CommonExceptions.AccessDenied(operation, StatusCodes.Status403Forbidden);
            }

            await userService.ManageUserStatusById(operation, id, true);
        });

        /// <summary>
        /// Deactivates user account
        /// </summary>
        /// <param name="id">Unique user ID</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpPut("{id}/deactivate")]
        public async Task<ActionResult> DeactivateUser(int id) => await Execute(OperationContexts.DeactivateUser, async operation =>
        {
            if (UserId != id && (UserPermissions & UserPermission.UpdateUserProfile) == 0)
            {
                throw CommonExceptions.AccessDenied(operation, StatusCodes.Status403Forbidden);
            }

            await userService.ManageUserStatusById(operation, id, false);
        });
    }
}