using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Utils.Authorization
{
    internal static class AuthorizationClaimName
    {
        internal const string UserId = "UserId";
    }

    public static class AuthorizationService
    {
        public static JwtSecurityToken GenerateAccessToken(AuthorizationTokenProperties authorizationTokenProperties, IEnumerable<Claim> claims)
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

        public static ClaimsIdentity GetIdentity(string id) => !string.IsNullOrWhiteSpace(id)
            ? new ClaimsIdentity(new List<Claim>
            {
                new Claim(AuthorizationClaimName.UserId, id)
            }, "Token")
            : null;

        public static string GetUserId(IEnumerable<Claim> claims) => GetClaimValue(claims, AuthorizationClaimName.UserId);

        private static string GetClaimValue(IEnumerable<Claim> claims, string key) => claims.FirstOrDefault(c => string.Equals(c.Type, key, StringComparison.InvariantCultureIgnoreCase))?.Value;

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public static ClaimsPrincipal GetClaimsPrincipalDataFromToken(string accessToken, AuthorizationTokenProperties authorizationTokenProperties)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, GetServerTokenValidationParameters(authorizationTokenProperties, false), out var securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(authorizationTokenProperties.SecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
            {
                throw CommonExceptions.InvalidTokenInHeader();
            }

            return principal;
        }

        public static TokenValidationParameters GetServerTokenValidationParameters(AuthorizationTokenProperties authorizationTokenProperties, bool validateLifetime = true) =>
            new TokenValidationParameters
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