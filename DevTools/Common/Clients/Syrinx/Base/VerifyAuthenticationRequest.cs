using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.Syrinx.Base
{
    public class VerifyAuthenticationRequest : IRequest
    {
        public Dictionary<string, string> Path { get; set; }
        public Dictionary<string, string> Query => null;
        public string Body => null;
        public string Token { get; set; }
    }
}