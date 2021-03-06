using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Services;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.User;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Controllers
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
        [ProducesResponseType(typeof(UserPagingResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [PortalAuthentication]
        [HttpPost("list")]
        public async Task<ActionResult<UserPagingResponseModel>> GetUsers([FromBody] UserPagingRequestModel model) => await Execute(async operation =>
        {
            var users = await userService.GetUsers(operation, model.ToEntity());
            var response = new UserPagingResponseModel().ToModel(users);
            return response;
        });

        /// <summary>
        /// Searches users by pattern
        /// </summary>
        /// <returns>List of users shortly</returns>
        [ProducesResponseType(typeof(IEnumerable<UserShortModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [PortalAuthentication]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserShortModel>>> SearchUsersByPattern([FromQuery(Name = "p")] string pattern, [FromQuery(Name = "z")] string zeroUserName)
        {
            return await Execute(async operation =>
            {
                var users = await userService.SearchUsersByPattern(operation, pattern, zeroUserName);
                return users.Select(user => user.ToModel());
            });
        }

        /// <summary>
        /// Obtains user profile data by ID
        /// </summary>
        /// <param name="id">Unique user ID</param>
        /// <returns>User data if it was found</returns>
        [ProducesResponseType(typeof(UserProfileGetModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileGetModel>> GetUserProfile(int id) => await Execute(async operation =>
        {
            var entity = await userService.GetUserProfileById(operation, id);
            return entity.ToModel();
        });

        /// <summary>
        /// Updates user profile data
        /// </summary>
        /// <param name="id">Unique user ID for update</param>
        /// <param name="model">Model of user profile for update</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [PortalAuthentication]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserInfo(int id, [FromBody] UserProfileUpdateModel model)
        {
            return await Execute(async operation =>
            {
                var portalPermissions = (PortalPermission) (UserPermissions.FirstOrDefault(item => item.PermissionGroupId == 1)?.Permissions ?? 0);
                if (UserId != id && (portalPermissions & PortalPermission.ManageUserProfiles) == 0)
                {
                    throw CommonExceptions.AccessDenied(operation, StatusCodes.Status403Forbidden);
                }

                var entity = model.ToEntity(id);
                await userService.UpdateUserProfile(operation, entity);
            });
        }
    }
}