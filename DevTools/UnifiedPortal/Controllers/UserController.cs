using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Services.Base;
using VXDesign.Store.DevTools.UnifiedPortal.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ApiController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Obtains user profile data by email
        /// </summary>
        /// <param name="email">Unique email address for user</param>
        /// <returns>User data if it was found</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpGet]
        public async Task<ActionResult<UserProfileModel>> GetUserProfile([FromQuery] string email) => await HandleExceptionIfThrown(async () =>
        {
            var entity = await userService.GetUserProfileByEmail(email);
            return entity.ToModel();
        });

        /// <summary>
        /// Updates user profile
        /// </summary>
        /// <param name="model">Model of user profile for update</param>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SyrinxVerifiedAuthentication]
        [HttpPut]
        public async Task<ActionResult> UpdateUserProfile([FromBody] UserProfileModel model) => await HandleExceptionIfThrown(async () =>
        {
            var entity = model.ToEntity();
            await userService.UpdateUserProfile(entity);
        });
    }
}