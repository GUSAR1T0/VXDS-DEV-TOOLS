using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.SRS.Authorization
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
        public double ExpireTimeInSeconds { get; set; } = TimeSpan.FromHours(1).TotalSeconds;

        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public string SecurityAlgorithm => SecurityAlgorithms.HmacSha512Signature;
    }
}