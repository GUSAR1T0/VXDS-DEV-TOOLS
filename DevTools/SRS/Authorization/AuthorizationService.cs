using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Storage;
using VXDesign.Store.DevTools.Common.Storage.DataStores;
using VXDesign.Store.DevTools.Common.Utils.Authorization;

namespace VXDesign.Store.DevTools.SRS.Authorization
{
    public interface IAuthorizationService
    {
        Task<RawJwtToken> SignIn(IOperation operation, string email, string password);
        Task<RawJwtToken> SignUp(IOperation operation, UserRegistrationEntity entity);
        Task<RawJwtToken> RefreshToken(IOperation operation, string accessToken, string refreshToken);
        Task Logout(IOperation operation, IEnumerable<Claim> claims);
        Task<UserAuthorizationEntity> GetUserData(IOperation operation, IEnumerable<Claim> claims);

        TokenValidationParameters GetServerTokenValidationParameters(bool validateLifetime = true);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly AuthorizationTokenProperties authorizationTokenProperties;
        private readonly IUserDataStore userDataStore;

        public AuthorizationService(AuthorizationTokenProperties authorizationTokenProperties, IUserDataStore userDataStore)
        {
            this.authorizationTokenProperties = authorizationTokenProperties;
            this.userDataStore = userDataStore;
        }

        public async Task<RawJwtToken> SignIn(IOperation operation, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw CommonExceptions.NoAuthenticationData();
            }

            var id = await userDataStore.GetIdByAccessData(operation, email, password);
            if (id == null)
            {
                throw CommonExceptions.UserWasNotFound(email);
            }

            var identity = GetIdentity(id);

            var token = new RawJwtToken
            {
                AccessToken = GenerateAccessToken(identity.Claims.ToList()),
                RefreshToken = AuthorizationUtils.GenerateRefreshToken()
            };

            await userDataStore.UpdateRefreshTokenById(operation, id.Value, token.RefreshToken);

            return token;
        }

        public async Task<RawJwtToken> SignUp(IOperation operation, UserRegistrationEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Email) || string.IsNullOrWhiteSpace(entity.Password))
            {
                throw CommonExceptions.NoAuthenticationData();
            }

            if (await userDataStore.GetIdByAccessData(operation, entity.Email) != null)
            {
                throw CommonExceptions.UserHasAlreadyExist();
            }

            var user = await userDataStore.CreateUser(operation, entity);
            if (user == null)
            {
                throw CommonExceptions.RegistrationIsFailed();
            }

            var identity = GetIdentity(user.Id);

            var token = new RawJwtToken
            {
                AccessToken = GenerateAccessToken(identity.Claims.ToList()),
                RefreshToken = AuthorizationUtils.GenerateRefreshToken()
            };

            await userDataStore.UpdateRefreshTokenById(operation, user.Id, token.RefreshToken);

            return token;
        }

        public async Task<RawJwtToken> RefreshToken(IOperation operation, string accessToken, string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                throw CommonExceptions.NoAuthenticationData();
            }

            var principal = GetClaimsPrincipalDataFromToken(accessToken);
            var claims = principal.Claims.ToList();
            var id = AuthorizationUtils.GetUserId(claims) ?? throw CommonExceptions.FailedToReadAuthenticationDataFromClaims();
            var storedRefreshToken = await userDataStore.GetRefreshTokenById(operation, id);
            if (storedRefreshToken?.Equals(refreshToken) != true) throw CommonExceptions.RefreshTokensAreDifferent();

            var token = new RawJwtToken
            {
                AccessToken = GenerateAccessToken(claims.ToList()),
                RefreshToken = AuthorizationUtils.GenerateRefreshToken()
            };

            await userDataStore.UpdateRefreshTokenById(operation, id, token.RefreshToken);

            return token;
        }

        public async Task Logout(IOperation operation, IEnumerable<Claim> claims)
        {
            var id = AuthorizationUtils.GetUserId(claims) ?? throw CommonExceptions.FailedToReadAuthenticationDataFromClaims();
            var identity = GetIdentity(id);
            identity?.Claims.ToList().ForEach(claim => identity.RemoveClaim(claim));
            await userDataStore.UpdateRefreshTokenById(operation, id, null);
        }

        public async Task<UserAuthorizationEntity> GetUserData(IOperation operation, IEnumerable<Claim> claims)
        {
            var id = AuthorizationUtils.GetUserId(claims) ?? throw CommonExceptions.FailedToReadAuthenticationDataFromClaims();
            return await userDataStore.GetAuthorizationById(operation, id);
        }

        private JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;
            return new JwtSecurityToken(
                issuer: authorizationTokenProperties.Issuer,
                audience: authorizationTokenProperties.Audience,
                notBefore: now,
                claims: claims,
                expires: now.AddSeconds(authorizationTokenProperties.ExpireTimeInSeconds),
                signingCredentials: new SigningCredentials(authorizationTokenProperties.SymmetricSecurityKey, authorizationTokenProperties.SecurityAlgorithm)
            );
        }

        private static ClaimsIdentity GetIdentity(int? id) => id.HasValue ? AuthorizationUtils.GetClaimsIdentity(id.ToString()) : null;

        private ClaimsPrincipal GetClaimsPrincipalDataFromToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, GetServerTokenValidationParameters(false), out var securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(authorizationTokenProperties.SecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
            {
                throw CommonExceptions.InvalidTokenInHeader();
            }

            return principal;
        }

        public TokenValidationParameters GetServerTokenValidationParameters(bool validateLifetime = true) => new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authorizationTokenProperties.Issuer,
            ValidateAudience = true,
            ValidAudience = authorizationTokenProperties.Audience,
            ValidateLifetime = validateLifetime,
            IssuerSigningKey = authorizationTokenProperties.SymmetricSecurityKey,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    }
}