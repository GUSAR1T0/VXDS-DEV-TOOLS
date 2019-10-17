using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.SRS.Syrinx.Extensions;
using VXDesign.Store.DevTools.SRS.Syrinx.Models.Authorization;
using IAuthorizationService = VXDesign.Store.DevTools.SRS.Authorization.IAuthorizationService;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ApiController
    {
        private readonly IAuthorizationService authorizationService;

        public AccountController(IOperationService operationService, IAuthorizationService authorizationService) : base(operationService)
        {
            this.authorizationService = authorizationService;
        }

        /// <summary>
        /// Authenticates some user and generates JSON Web Token
        /// </summary>
        /// <param name="model">Authentication model with needed fields</param>
        /// <returns>String values of access and refresh tokens</returns>
        [ProducesResponseType(typeof(JwtTokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<ActionResult<JwtTokenModel>> SignIn([FromBody] SignInModel model) => await Execute(async operation =>
        {
            var token = await authorizationService.SignIn(model.Email, model.Password);
            return token.GetJwtTokenModel();
        });

        /// <summary>
        /// Registers new user and generates JSON Web Token
        /// </summary>
        /// <param name="model">Registration model with needed fields</param>
        /// <returns>String values of access and refresh tokens</returns>
        [ProducesResponseType(typeof(JwtTokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<ActionResult<JwtTokenModel>> SignUp([FromBody] SignUpModel model) => await Execute(async operation =>
        {
            var token = await authorizationService.SignUp(model.ToEntity());
            return token.GetJwtTokenModel();
        });

        /// <summary>
        /// Generates JSON Web Token for user
        /// </summary>
        /// <param name="model">Refresh token model with needed fields</param>
        /// <returns>String values of access and refresh tokens</returns>
        [ProducesResponseType(typeof(JwtTokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<JwtTokenModel>> RefreshToken([FromBody] JwtTokenModel model) => await Execute(async operation =>
        {
            var token = await authorizationService.RefreshToken(model.AccessToken, model.RefreshToken);
            return token.GetJwtTokenModel();
        });

        /// <summary>
        /// Revokes user JSON Web Token to perform logout
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout() => await Execute(async context => await authorizationService.Logout(User.Claims));

        /// <summary>
        /// Obtains authorization user data by token
        /// </summary>
        /// <returns>Authorization user data</returns>
        [ProducesResponseType(typeof(UserAuthorizationModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserAuthorizationModel>> GetUserData() => await Execute(async operation =>
        {
            var userData = await authorizationService.GetUserData(User.Claims);
            return userData.ToModel();
        });

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