using System.Collections.Generic;
using VXDesign.Store.DevTools.Core.Entities.HTTP;

namespace VXDesign.Store.DevTools.Core.Entities.Syrinx
{
    public class VerifyAuthenticationRequest : IRequest
    {
        public Dictionary<string, string> Path { get; set; }
        public Dictionary<string, string> Query => null;
        public string Body => null;
        public string Token { get; set; }
    }
}