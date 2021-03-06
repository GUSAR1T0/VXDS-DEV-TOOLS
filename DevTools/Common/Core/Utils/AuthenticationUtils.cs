using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.User;

namespace VXDesign.Store.DevTools.Common.Core.Utils
{
    public static class AuthenticationUtils
    {
        public static int? GetUserId(IEnumerable<Claim> claims)
        {
            return int.TryParse(GetClaimValue(claims, AuthenticationClaimName.UserId), out var value) ? value : (int?) null;
        }

        public static IEnumerable<UserRolePermissionEntity> GetUserPermissions(IEnumerable<Claim> claims)
        {
            return JsonConvert.DeserializeObject<IEnumerable<UserRolePermissionEntity>>(GetClaimValue(claims, AuthenticationClaimName.UserPermissions));
        }

        private static string GetClaimValue(IEnumerable<Claim> claims, string key)
        {
            return claims.FirstOrDefault(c => string.Equals(c.Type, key, StringComparison.InvariantCultureIgnoreCase))?.Value;
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public static ClaimsIdentity GetClaimsIdentity(UserAuthorizationEntity entity) => GetClaimsIdentity(new List<Claim>
        {
            new Claim(AuthenticationClaimName.UserId, entity.Id.ToString()),
            new Claim(AuthenticationClaimName.UserPermissions, JsonConvert.SerializeObject(entity.Permissions))
        });

        public static ClaimsIdentity GetClaimsIdentity(IEnumerable<Claim> claims) => new ClaimsIdentity(claims, "Token");
    }
}