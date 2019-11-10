using VXDesign.Store.DevTools.Core.Entities.HTTP;

namespace VXDesign.Store.DevTools.Core.Entities.Syrinx
{
    public class VerifyAuthenticationResponse : IResponse
    {
        public int Status { get; set; }
        public string Output { get; set; }
        public string Reason { get; set; }
    }
}