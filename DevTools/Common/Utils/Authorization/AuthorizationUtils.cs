using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using VXDesign.Store.DevTools.Common.Entities.Authorization;

namespace VXDesign.Store.DevTools.Common.Utils.Authorization
{
    public class AuthorizationUtils
    {
        public static int? GetUserId(IEnumerable<Claim> claims) => int.TryParse(GetClaimValue(claims, AuthorizationClaimName.UserId), out var value) ? value : (int?) null;

        public static string GetClaimValue(IEnumerable<Claim> claims, string key) => claims.FirstOrDefault(c => string.Equals(c.Type, key, StringComparison.InvariantCultureIgnoreCase))?.Value;

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public static ClaimsIdentity GetClaimsIdentity(string userId) => new ClaimsIdentity(new List<Claim>
        {
            new Claim(AuthorizationClaimName.UserId, userId)
        }, "Token");
    }
}