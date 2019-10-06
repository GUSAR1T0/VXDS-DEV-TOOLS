using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Entities.HTTP;

namespace VXDesign.Store.DevTools.Common.Containers.Syrinx.VerifyAuthentication
{
    public class VerifyAuthenticationRequest : IRequest
    {
        public Dictionary<string, string> Path { get; set; }
        public Dictionary<string, string> Query => null;
        public string Body => null;
        public string Token { get; set; }
    }
}