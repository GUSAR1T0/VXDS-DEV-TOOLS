using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.Common.Utils.Authentication;
using VXDesign.Store.DevTools.SRS.Authentication;
using VXDesign.Store.DevTools.SRS.Syrinx.Extensions;
using VXDesign.Store.DevTools.SRS.Syrinx.Models.Authorization;
using VXDesign.Store.DevTools.SRS.Syrinx.Utils;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ApiController
    {
        private readonly IAuthenticationService authenticationService;

        public AccountController(IOperationService operationService, IAuthenticationService authenticationService) : base(operationService)
        {
            this.authenticationService = authenticationService;
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
        public async Task<ActionResult<JwtTokenModel>> SignIn([FromBody] SignInModel model) => await Execute(OperationContexts.SignIn, async operation =>
        {
            var token = await authenticationService.SignIn(operation, model.Email, model.Password);
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
        public async Task<ActionResult<JwtTokenModel>> SignUp([FromBody] SignUpModel model) => await Execute(OperationContexts.SignUp, async operation =>
        {
            var token = await authenticationService.SignUp(operation, model.ToEntity());
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
        public async Task<ActionResult<JwtTokenModel>> RefreshToken([FromBody] JwtTokenModel model) => await Execute(OperationContexts.RefreshToken, async operation =>
        {
            var token = await authenticationService.RefreshToken(operation, model.AccessToken, model.RefreshToken);
            return token.GetJwtTokenModel();
        });

        /// <summary>
        /// Revokes user JSON Web Token to perform logout
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout() => await Execute(OperationContexts.Logout, async operation => await authenticationService.Logout(operation, User.Claims));

        /// <summary>
        /// Obtains authorization user data by token
        /// </summary>
        /// <returns>Authorization user data</returns>
        [ProducesResponseType(typeof(UserAuthorizationModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserAuthorizationModel>> GetUserData() => await Execute(OperationContexts.GetUserData, async operation =>
        {
            var userData = await authenticationService.GetUserData(operation, User.Claims);
            return userData.ToModel();
        });

        /// <summary>
        /// Handles queries from derived servers to send available session permissions for user
        /// </summary>
        /// <returns>Authorization user data (user ID and permissions only) if token is validated</returns>
        [ProducesResponseType(typeof(UserAuthorizationModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet("verify")]
        public async Task<ActionResult<UserAuthorizationModel>> VerifyAuthentication() => await Execute(OperationContexts.VerifyAuthentication, async operation =>
        {
            var userId = AuthenticationUtils.GetUserId(User.Claims);
            if (userId == null)
            {
                throw CommonExceptions.AccessDenied(operation, StatusCodes.Status401Unauthorized);
            }

            return new UserAuthorizationModel
            {
                Id = userId.Value,
                UserPermissions = AuthenticationUtils.GetUserPermissions(User.Claims),
                UserRolePermissions = AuthenticationUtils.GetUserRolePermissions(User.Claims)
            };
        });
    }
}