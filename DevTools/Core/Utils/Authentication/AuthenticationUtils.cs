using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using VXDesign.Store.DevTools.Core.Entities.Authentication;
using VXDesign.Store.DevTools.Core.Entities.Storage;
using VXDesign.Store.DevTools.Core.Enums.Operations;

namespace VXDesign.Store.DevTools.Core.Utils.Authentication
{
    public static class AuthenticationUtils
    {
        public static int? GetUserId(IEnumerable<Claim> claims)
        {
            return int.TryParse(GetClaimValue(claims, AuthenticationClaimName.UserId), out var value) ? value : (int?) null;
        }

        public static UserPermission GetUserPermissions(IEnumerable<Claim> claims)
        {
            return Enum.TryParse<UserPermission>(GetClaimValue(claims, AuthenticationClaimName.UserPermissions), out var value) ? value : 0;
        }

        private static string GetClaimValue(IEnumerable<Claim> claims, string key)
        {
            return claims.FirstOrDefault(c => string.Equals(c.Type, key, StringComparison.InvariantCultureIgnoreCase))?.Value;
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public static ClaimsIdentity GetClaimsIdentity(UserAuthorizationEntity entity) => GetClaimsIdentity(new List<Claim>
        {
            new Claim(AuthenticationClaimName.UserId, entity.Id.ToString()),
            new Claim(AuthenticationClaimName.UserPermissions, entity.UserPermissions.ToString("D"))
        });

        public static ClaimsIdentity GetClaimsIdentity(IEnumerable<Claim> claims) => new ClaimsIdentity(claims, "Token");
    }
}