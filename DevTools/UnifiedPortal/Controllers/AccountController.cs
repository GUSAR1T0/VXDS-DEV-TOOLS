using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Models.Authorization;
using VXDesign.Store.DevTools.Common.Utils.Authorization;
using VXDesign.Store.DevTools.Common.Utils.DataStorage;
using VXDesign.Store.DevTools.UnifiedPortal.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Properties;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ApiController
    {
        private readonly PortalProperties properties;
        private readonly IUserDataService userDataService;

        public AccountController(PortalProperties properties, IUserDataService userDataService)
        {
            this.properties = properties;
            this.userDataService = userDataService;
        }

        /// <summary>
        /// Generates JSON Web Token for user
        /// </summary>
        /// <param name="model">Authentication model with needed fields</param>
        /// <returns>String values of access and refresh tokens</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<ActionResult<JwtTokenModel>> GenerateToken([FromBody] AuthenticationModel model) => await HandleExceptionIfThrown(async () =>
        {
            IEnumerable<Claim> claims;
            string id;

            if (!string.IsNullOrWhiteSpace(model.AccessToken) && !string.IsNullOrWhiteSpace(model.RefreshToken))
            {
                var principal = AuthorizationService.GetClaimsPrincipalDataFromToken(model.AccessToken, properties.AuthorizationTokenProperties);
                claims = principal.Claims.ToList();
                id = AuthorizationService.GetUserId(claims);
                var storedRefreshToken = await userDataService.GetRefreshTokenById(id);
                if (storedRefreshToken?.Equals(model.RefreshToken) != true) throw CommonExceptions.RefreshTokensAreDifferent();
            }
            else if (!string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.Password))
            {
                id = await userDataService.GetIdByUser(model.Email, model.Password);
                var identity = AuthorizationService.GetIdentity(id);
                if (identity == null) throw CommonExceptions.InvalidEmailOrPassword();
                claims = identity.Claims.ToList();
            }
            else throw CommonExceptions.NoAuthenticationData();

            var accessToken = AuthorizationService.GenerateAccessToken(properties.AuthorizationTokenProperties, claims);
            var refreshToken = AuthorizationService.GenerateRefreshToken();

            await userDataService.UpdateRefreshToken(id, refreshToken);

            return new JwtTokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken
            };
        });

        /// <summary>
        /// Register new user and generates JSON Web Token for it
        /// </summary>
        /// <param name="model">Registration model with needed fields</param>
        /// <returns>String values of access and refresh tokens</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<JwtTokenModel>> RegisterUser([FromBody] RegistrationModel model) => await HandleExceptionIfThrown(async () =>
        {
            if (await userDataService.GetIdByUser(model.Email) != null) throw CommonExceptions.UserHasAlreadyExist();

            var entity = await userDataService.Create(model.ToEntity());
            var identity = AuthorizationService.GetIdentity(entity.Id);
            if (identity == null) throw CommonExceptions.InvalidEmailOrPassword();
            var claims = identity.Claims.ToList();

            var accessToken = AuthorizationService.GenerateAccessToken(properties.AuthorizationTokenProperties, claims);
            var refreshToken = AuthorizationService.GenerateRefreshToken();

            await userDataService.UpdateRefreshToken(entity.Id, refreshToken);

            return new JwtTokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken
            };
        });

        /// <summary>
        /// Revokes user JSON Web Token to perform logout
        /// </summary>
        /// <returns>Nothing to return</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpDelete("token")]
        public async Task RevokeToken()
        {
            var id = AuthorizationService.GetUserId(User.Claims);
            var identity = AuthorizationService.GetIdentity(id);
            identity.Claims.ToList().ForEach(claim => identity.RemoveClaim(claim));
            await userDataService.UpdateRefreshToken(id, null);
        }

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
            var id = AuthorizationService.GetUserId(User.Claims);
            var entity = await userDataService.GetEntityById(id);
            return entity.ToModel();
        }
    }
}