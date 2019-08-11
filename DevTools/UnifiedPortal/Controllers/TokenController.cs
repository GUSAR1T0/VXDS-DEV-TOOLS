using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Models.Authorization;
using VXDesign.Store.DevTools.Common.Utils.Authorization;
using VXDesign.Store.DevTools.UnifiedPortal.Properties;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : ApiController
    {
        private static readonly Dictionary<string, string> UserTokens = new Dictionary<string, string>();

        private readonly PortalProperties properties;

        public TokenController(PortalProperties properties)
        {
            this.properties = properties;
        }

        /// <summary>
        /// Generates JSON Web Token for application
        /// </summary>
        /// <param name="model">Authentication model with needed fields</param>
        /// <returns>String value of token</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<JwtTokenModel> Token([FromBody] AuthenticationModel model) => HandleExceptionIfThrown<JwtTokenModel>(() =>
        {
            IEnumerable<Claim> claims;
            if (!string.IsNullOrWhiteSpace(model.AccessToken) && !string.IsNullOrWhiteSpace(model.RefreshToken))
            {
                var principal = AuthorizationService.GetClaimsPrincipalDataFromToken(model.AccessToken, properties.AuthorizationTokenProperties);
                claims = principal.Claims.ToList();
                model.Email = AuthorizationService.GetUserData(claims).Email;
                var storedRefreshToken = UserTokens.ContainsKey(model.Email) ? UserTokens[model.Email] : null;
                if (storedRefreshToken?.Equals(model.RefreshToken) != true) throw CommonExceptions.RefreshTokensAreDifferent();
            }
            else if (!string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.Password))
            {
                var identity = AuthorizationService.GetIdentity(model.Email, model.Password);
                if (identity == null) throw CommonExceptions.InvalidEmailOrPassword();
                claims = identity.Claims.ToList();
            }
            else throw CommonExceptions.NoAuthenticationData();

            var accessToken = AuthorizationService.GenerateAccessToken(properties.AuthorizationTokenProperties, claims);
            var refreshToken = AuthorizationService.GenerateRefreshToken();

            // TODO: Restore refresh token for user in DB
            UserTokens[model.Email] = refreshToken;

            return new JwtTokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken
            };
        });

        /// <summary>
        /// Obtains authorization user data by token
        /// </summary>
        /// <returns>Authorization user data</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        [HttpGet("user")]
        public AuthorizationUserModel GetUserData() => AuthorizationService.GetUserData(User.Claims);
    }
}