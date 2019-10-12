using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.SRS.Syrinx.Extensions;
using VXDesign.Store.DevTools.SRS.Syrinx.Models.Authorization;
using IAuthorizationService = VXDesign.Store.DevTools.Common.Services.Authorization.IAuthorizationService;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ApiController
    {
        private readonly IAuthorizationService authorizationService;

        public AccountController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        /// <summary>
        /// Authenticates some user and generates JSON Web Token
        /// </summary>
        /// <param name="model">Authentication model with needed fields</param>
        /// <returns>String values of access and refresh tokens</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<ActionResult<JwtTokenModel>> SignIn([FromBody] SignInModel model) => await HandleExceptionIfThrown(async () =>
        {
            var token = await authorizationService.SignIn(model.Email, model.Password);
            return token.GetJwtTokenModel();
        });

        /// <summary>
        /// Registers new user and generates JSON Web Token
        /// </summary>
        /// <param name="model">Registration model with needed fields</param>
        /// <returns>String values of access and refresh tokens</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<ActionResult<JwtTokenModel>> SignUp([FromBody] SignUpModel model) => await HandleExceptionIfThrown(async () =>
        {
            var token = await authorizationService.SignUp(model.ToEntity());
            return token.GetJwtTokenModel();
        });

        /// <summary>
        /// Generates JSON Web Token for user
        /// </summary>
        /// <param name="model">Refresh token model with needed fields</param>
        /// <returns>String values of access and refresh tokens</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<JwtTokenModel>> RefreshToken([FromBody] JwtTokenModel model) => await HandleExceptionIfThrown(async () =>
        {
            var token = await authorizationService.RefreshToken(model.AccessToken, model.RefreshToken);
            return token.GetJwtTokenModel();
        });

        /// <summary>
        /// Revokes user JSON Web Token to perform logout
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPost("logout")]
        public async Task Logout() => await authorizationService.Logout(User.Claims);

        /// <summary>
        /// Obtains authorization user data by token
        /// </summary>
        /// <returns>Authorization user data</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet]
        public async Task<UserAuthorizationModel> GetUserData()
        {
            var userData = await authorizationService.GetUserData(User.Claims);
            return userData.ToModel();
        }

        /// <summary>
        /// Tests authentication token from derived servers
        /// </summary>
        /// <returns><c>True</c> if token is validated</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet("verify")]
        public bool VerifyAuthentication() => true;
    }
}