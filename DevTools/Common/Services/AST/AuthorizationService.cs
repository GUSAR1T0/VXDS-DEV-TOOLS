using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using VXDesign.Store.DevTools.Common.Containers.AST.Authorization;
using VXDesign.Store.DevTools.Common.Containers.Properties;
using VXDesign.Store.DevTools.Common.DataStorage.Entities;
using VXDesign.Store.DevTools.Common.DataStorage.Stores;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;

namespace VXDesign.Store.DevTools.Common.Services.AST
{
    internal static class AuthorizationClaimName
    {
        internal const string UserId = "UserId";
    }

    public interface IAuthorizationService
    {
        Task<RawJwtToken> SignIn(string email, string password);
        Task<RawJwtToken> SignUp(UserRegistrationEntity entity);
        Task<RawJwtToken> RefreshToken(string accessToken, string refreshToken);
        Task Logout(IEnumerable<Claim> claims);
        Task<UserAuthorizationEntity> GetUserData(IEnumerable<Claim> claims);

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

        public async Task<RawJwtToken> SignIn(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw CommonExceptions.NoAuthenticationData();
            }

            var id = await userDataStore.GetIdByAccessData(email, password);
            if (id == null)
            {
                throw CommonExceptions.InvalidEmailOrPassword();
            }

            var identity = GetIdentity(id);

            var token = new RawJwtToken
            {
                AccessToken = GenerateAccessToken(identity.Claims.ToList()),
                RefreshToken = GenerateRefreshToken()
            };

            await userDataStore.UpdateRefreshTokenById(id, token.RefreshToken);

            return token;
        }

        public async Task<RawJwtToken> SignUp(UserRegistrationEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Email) || string.IsNullOrWhiteSpace(entity.Password))
            {
                throw CommonExceptions.NoAuthenticationData();
            }

            if (await userDataStore.GetIdByAccessData(entity.Email) != null)
            {
                throw CommonExceptions.UserHasAlreadyExist();
            }

            UserAuthorizationEntity user;
            try
            {
                user = await userDataStore.CreateUser(entity);
            }
            catch (Exception e)
            {
                throw CommonExceptions.RegistrationIsFailed(e.Message);
            }

            var identity = GetIdentity(user.Id);

            var token = new RawJwtToken
            {
                AccessToken = GenerateAccessToken(identity.Claims.ToList()),
                RefreshToken = GenerateRefreshToken()
            };

            await userDataStore.UpdateRefreshTokenById(user.Id, token.RefreshToken);

            return token;
        }

        public async Task<RawJwtToken> RefreshToken(string accessToken, string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                throw CommonExceptions.NoAuthenticationData();
            }

            var principal = GetClaimsPrincipalDataFromToken(accessToken);
            var claims = principal.Claims.ToList();
            var id = GetUserId(claims);
            var storedRefreshToken = await userDataStore.GetRefreshTokenById(id);
            if (storedRefreshToken?.Equals(refreshToken) != true) throw CommonExceptions.RefreshTokensAreDifferent();

            var token = new RawJwtToken
            {
                AccessToken = GenerateAccessToken(claims.ToList()),
                RefreshToken = GenerateRefreshToken()
            };

            await userDataStore.UpdateRefreshTokenById(id, token.RefreshToken);

            return token;
        }

        public async Task Logout(IEnumerable<Claim> claims)
        {
            var id = GetUserId(claims);
            var identity = GetIdentity(id);
            identity?.Claims.ToList().ForEach(claim => identity.RemoveClaim(claim));
            await userDataStore.UpdateRefreshTokenById(id, null);
        }

        public async Task<UserAuthorizationEntity> GetUserData(IEnumerable<Claim> claims)
        {
            var id = GetUserId(claims);
            return await userDataStore.GetAuthorizationById(id);
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

        private static ClaimsIdentity GetIdentity(string id) => !string.IsNullOrWhiteSpace(id)
            ? new ClaimsIdentity(new List<Claim>
            {
                new Claim(AuthorizationClaimName.UserId, id)
            }, "Token")
            : null;

        private static string GetUserId(IEnumerable<Claim> claims) => GetClaimValue(claims, AuthorizationClaimName.UserId);

        private static string GetClaimValue(IEnumerable<Claim> claims, string key) => claims.FirstOrDefault(c => string.Equals(c.Type, key, StringComparison.InvariantCultureIgnoreCase))?.Value;

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

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