using Microsoft.IdentityModel.Tokens;

namespace VXDesign.Store.DevTools.SRS.Authentication
{
    public class RawJwtToken
    {
        public SecurityToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}