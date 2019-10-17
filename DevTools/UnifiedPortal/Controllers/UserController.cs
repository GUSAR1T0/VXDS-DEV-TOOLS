using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.Common.Services.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;

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
        /// <param name="email">Unique email address for user</param>
        /// <returns>User data if it was found</returns>
        [ProducesResponseType(typeof(UserProfileGetModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpGet]
        public async Task<ActionResult<UserProfileGetModel>> GetUserProfile([FromQuery] string email) => await Execute(async operation =>
        {
            var entity = await userService.GetUserProfileByEmail(email);
            return entity.ToModel();
        });

        /// <summary>
        /// Updates user profile data
        /// </summary>
        /// <param name="id">ID of user for update</param>
        /// <param name="model">Model of user profile for update</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserProfile(int id, [FromBody] UserProfileGeneralInfoUpdateModel model) => await Execute(async operation =>
        {
            var entity = model.ToEntity(id);
            await userService.UpdateUserProfile(entity);
        });
    }
}