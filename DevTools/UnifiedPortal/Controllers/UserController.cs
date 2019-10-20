using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.Common.Services.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;
using VXDesign.Store.DevTools.UnifiedPortal.Utils;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ApiController
    {
        private readonly IUserService userService;

        public UserController(IOperationService operationService, IUserService userService) : base(operationService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Obtains user profile data by email
        /// </summary>
        /// <param name="id">Unique user ID</param>
        /// <returns>User data if it was found</returns>
        [ProducesResponseType(typeof(UserProfileGetModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpPut("{id}/general")]
        public async Task<ActionResult> UpdateUserProfileGeneralInfo(int id, [FromBody] UserProfileGeneralInfoUpdateModel model)
        {
            return await Execute(OperationContexts.UpdateUserProfileGeneralInfo, async operation =>
            {
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpPut("{id}/accountSpecific")]
        public async Task<ActionResult> UpdateUserProfileAccountSpecificInfo(int id, [FromBody] UserProfileAccountSpecificInfoUpdateModel model)
        {
            return await Execute(OperationContexts.UpdateUserProfileAccountSpecificInfo, async operation =>
            {
                var entity = model.ToEntity(id);
                await userService.UpdateUserProfileAccountSpecificInfo(operation, entity);
            });
        }
    }
}