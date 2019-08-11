using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using VXDesign.Store.DevTools.Common.Containers.Properties;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Models.Authorization;

namespace VXDesign.Store.DevTools.Common.Utils.Authorization
{
    internal static class AuthorizationClaimName
    {
        internal const string Email = "Email";
        internal const string FirstName = "First Name";
        internal const string LastName = "Last Name";
    }

    public static class AuthorizationService
    {
        // TODO: Move to DB
        private const string FirstName = "Roman";
        private const string LastName = "Mashenkin";
        private const string Email = "vxdesign";
        private const string Password = "vxdesign";

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

        public static ClaimsIdentity GetIdentity(string email, string password)
        {
            return !Email.Equals(email) || !Password.Equals(password)
                ? null
                : new ClaimsIdentity(new List<Claim>
                {
                    new Claim(AuthorizationClaimName.Email, Email),
                    new Claim(AuthorizationClaimName.FirstName, FirstName),
                    new Claim(AuthorizationClaimName.LastName, LastName)
                }, "Token");
        }

        public static AuthorizationUserModel GetUserData(IEnumerable<Claim> claims) => new AuthorizationUserModel
        {
            Email = GetClaimValue(claims, AuthorizationClaimName.Email),
            FirstName = GetClaimValue(claims, AuthorizationClaimName.FirstName),
            LastName = GetClaimValue(claims, AuthorizationClaimName.LastName)
        };

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
            var principal = tokenHandler.ValidateToken(accessToken, GetServerTokenValidationParameters(authorizationTokenProperties), out var securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(authorizationTokenProperties.SecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
            {
                throw CommonExceptions.InvalidTokenInHeader();
            }

            return principal;
        }

        public static TokenValidationParameters GetServerTokenValidationParameters(AuthorizationTokenProperties authorizationTokenProperties) => new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authorizationTokenProperties.Issuer,
            ValidateAudience = true,
            ValidAudience = authorizationTokenProperties.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = authorizationTokenProperties.SymmetricSecurityKey,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    }
}