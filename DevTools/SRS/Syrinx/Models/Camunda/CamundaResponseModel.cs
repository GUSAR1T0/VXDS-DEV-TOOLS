using VXDesign.Store.DevTools.SRS.Camunda.Entities.API;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Models.Camunda
{
    public class CamundaResponseModel
    {
        public int Status { get; set; }
        public string Result { get; set; }
        
        internal static CamundaResponseModel Transform(CamundaResponse response) => new CamundaResponseModel
        {
            Status = response.Status,
            Result = response.Result
        };
    }
}