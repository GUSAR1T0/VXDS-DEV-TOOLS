using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using VXDesign.Store.DevTools.Common.Core.Entities.User;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Utils;
using VXDesign.Store.DevTools.Core.Entities.Operations;

namespace VXDesign.Store.DevTools.SRS.Authentication
{
    public interface IAuthenticationService
    {
        Task<RawJwtToken> SignIn(IOperation operation, string email, string password);
        Task<RawJwtToken> SignUp(IOperation operation, UserRegistrationEntity entity);
        Task<RawJwtToken> RefreshToken(IOperation operation, string accessToken, string refreshToken);
        Task Logout(IOperation operation, IEnumerable<Claim> claims);
        Task<UserAuthorizationEntity> GetUserData(IOperation operation, IEnumerable<Claim> claims);
        Task<bool> IsUserActivated(IOperation operation, int id);

        TokenValidationParameters GetServerTokenValidationParameters(bool validateLifetime = true);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationTokenProperties authenticationTokenProperties;
        private readonly IUserDataStore userDataStore;

        public AuthenticationService(AuthenticationTokenProperties authenticationTokenProperties, IUserDataStore userDataStore)
        {
            this.authenticationTokenProperties = authenticationTokenProperties;
            this.userDataStore = userDataStore;
        }

        public async Task<RawJwtToken> SignIn(IOperation operation, string email, string password)
        {
            var userIdentityClaims = await userDataStore.GetUserIdentityClaimsByAccessData(operation, email, password);
            if (userIdentityClaims == null)
            {
                throw CommonExceptions.AuthenticationFailed(operation);
            }

            if (!await userDataStore.IsUserActivated(operation, userIdentityClaims.Id))
            {
                throw CommonExceptions.AccessDenied(operation, StatusCodes.Status401Unauthorized, true);
            }

            var identity = GetIdentity(userIdentityClaims);

            var token = new RawJwtToken
            {
                AccessToken = GenerateAccessToken(identity.Claims.ToList()),
                RefreshToken = AuthenticationUtils.GenerateRefreshToken()
            };

            await userDataStore.UpdateRefreshTokenById(operation, userIdentityClaims.Id, token.RefreshToken);

            return token;
        }

        public async Task<RawJwtToken> SignUp(IOperation operation, UserRegistrationEntity entity)
        {
            if (await userDataStore.GetUserIdentityClaimsByAccessData(operation, entity.Email) != null)
            {
                throw CommonExceptions.UserHasAlreadyExist(operation);
            }

            var user = await userDataStore.CreateUser(operation, entity);
            if (user == null)
            {
                throw CommonExceptions.RegistrationIsFailed(operation);
            }

            var identity = GetIdentity(user);

            var token = new RawJwtToken
            {
                AccessToken = GenerateAccessToken(identity.Claims.ToList()),
                RefreshToken = AuthenticationUtils.GenerateRefreshToken()
            };

            await userDataStore.UpdateRefreshTokenById(operation, user.Id, token.RefreshToken);

            return token;
        }

        public async Task<RawJwtToken> RefreshToken(IOperation operation, string accessToken, string refreshToken)
        {
            var principal = GetClaimsPrincipalDataFromToken(operation, accessToken);
            var claims = principal.Claims.ToList();
            var id = AuthenticationUtils.GetUserId(claims) ?? throw CommonExceptions.FailedToReadAuthenticationDataFromClaims(operation);

            if (!await userDataStore.IsUserActivated(operation, id))
            {
                throw CommonExceptions.AccessDenied(operation, StatusCodes.Status401Unauthorized, true);
            }

            var storedRefreshToken = await userDataStore.GetRefreshTokenById(operation, id);
            if (storedRefreshToken?.Equals(refreshToken) != true)
            {
                throw CommonExceptions.RefreshTokensAreDifferent(operation);
            }

            var user = await userDataStore.GetUserIdentityClaimsById(operation, id);
            if (user == null)
            {
                throw CommonExceptions.AuthenticationFailed(operation);
            }

            var identity = GetIdentity(user);

            var token = new RawJwtToken
            {
                AccessToken = GenerateAccessToken(identity.Claims.ToList()),
                RefreshToken = AuthenticationUtils.GenerateRefreshToken()
            };

            await userDataStore.UpdateRefreshTokenById(operation, id, token.RefreshToken);

            return token;
        }

        public async Task Logout(IOperation operation, IEnumerable<Claim> claims)
        {
            var claimsList = claims.ToList();
            var id = AuthenticationUtils.GetUserId(claimsList) ?? throw CommonExceptions.FailedToReadAuthenticationDataFromClaims(operation);
            var identity = GetIdentity(claimsList);
            identity?.Claims.ToList().ForEach(claim => identity.RemoveClaim(claim));
            await userDataStore.UpdateRefreshTokenById(operation, id, null);
        }

        public async Task<UserAuthorizationEntity> GetUserData(IOperation operation, IEnumerable<Claim> claims)
        {
            var id = AuthenticationUtils.GetUserId(claims) ?? throw CommonExceptions.FailedToReadAuthenticationDataFromClaims(operation);
            return await userDataStore.GetAuthorizationById(operation, id);
        }

        public async Task<bool> IsUserActivated(IOperation operation, int id) => await userDataStore.IsUserActivated(operation, id);

        private JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;
            return new JwtSecurityToken(
                issuer: authenticationTokenProperties.Issuer,
                audience: authenticationTokenProperties.Audience,
                notBefore: now,
                claims: claims,
                expires: now.AddSeconds(authenticationTokenProperties.ExpireTimeInSeconds),
                signingCredentials: new SigningCredentials(authenticationTokenProperties.SymmetricSecurityKey, authenticationTokenProperties.SecurityAlgorithm)
            );
        }

        private static ClaimsIdentity GetIdentity(UserAuthorizationEntity entity) => entity != null ? AuthenticationUtils.GetClaimsIdentity(entity) : null;

        private static ClaimsIdentity GetIdentity(IReadOnlyCollection<Claim> claims) => claims?.Count == 3 ? AuthenticationUtils.GetClaimsIdentity(claims) : null;

        private ClaimsPrincipal GetClaimsPrincipalDataFromToken(IOperation operation, string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, GetServerTokenValidationParameters(false), out var securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(authenticationTokenProperties.SecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
            {
                throw CommonExceptions.InvalidTokenInHeader(operation);
            }

            return principal;
        }

        public TokenValidationParameters GetServerTokenValidationParameters(bool validateLifetime = true) => new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authenticationTokenProperties.Issuer,
            ValidateAudience = true,
            ValidAudience = authenticationTokenProperties.Audience,
            ValidateLifetime = validateLifetime,
            IssuerSigningKey = authenticationTokenProperties.SymmetricSecurityKey,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    }
}