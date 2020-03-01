using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.SRS.Camunda
{
    public class CamundaResponse : IResponse
    {
        public int Status { get; set; }
        public string Output { get; set; }
        public string Reason { get; set; }
    }
}