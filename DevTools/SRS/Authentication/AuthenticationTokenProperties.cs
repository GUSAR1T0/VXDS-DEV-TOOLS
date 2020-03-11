using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VXDesign.Store.DevTools.Common.Core.Properties;

namespace VXDesign.Store.DevTools.SRS.Authentication
{
    public class AuthenticationTokenProperties : IPropertiesMarker
    {
        [PropertyField]
        public string Issuer { get; set; }

        [PropertyField]
        public string Audience { get; set; }

        [PropertyField]
        public string SecretKey { get; set; }

        [PropertyField(Key = "ExpireTime")]
        public double ExpireTimeInSeconds { get; set; } = TimeSpan.FromMinutes(10).TotalSeconds;

        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public string SecurityAlgorithm => SecurityAlgorithms.HmacSha512Signature;
    }
}