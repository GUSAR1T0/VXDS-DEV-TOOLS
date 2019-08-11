using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Containers.Properties
{
    public class AuthorizationTokenProperties : IPropertiesMarker
    {
        [PropertyField]
        public string Issuer { get; set; }

        [PropertyField]
        public string Audience { get; set; }

        [PropertyField]
        public string SecretKey { get; set; }

        [PropertyField(Key = "ExpireTime")]
        public double ExpireTimeInSeconds { get; set; } = TimeSpan.FromMinutes(5).TotalSeconds;

        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public string SecurityAlgorithm => SecurityAlgorithms.HmacSha512Signature;
    }
}