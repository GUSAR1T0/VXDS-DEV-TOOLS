using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.Syrinx.Base
{
    public class VerifyAuthenticationResponse : IResponse
    {
        public int Status { get; set; }
        public string Output { get; set; }
        public string Reason { get; set; }
    }
}