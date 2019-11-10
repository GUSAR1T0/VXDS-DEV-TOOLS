using VXDesign.Store.DevTools.Core.Entities.Camunda.Base;
using VXDesign.Store.DevTools.SRS.Camunda;
using VXDesign.Store.DevTools.SRS.Syrinx.Models.Camunda;
using CamundaRequestModel = VXDesign.Store.DevTools.SRS.Syrinx.Models.Camunda.CamundaRequestModel;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Extensions
{
    internal static class CamundaModelExtensions
    {
        internal static CamundaResponseModel ToModel(this CamundaResponse response) => new CamundaResponseModel
        {
            Status = response.Status,
            Output = response.Output
        };

        internal static CamundaRequest ToEntity(this CamundaRequestModel model, CamundaEndpoint endpoint) => new CamundaRequest
        {
            Endpoint = endpoint,
            Path = model.Path,
            Query = model.Query,
            Body = model.Body
        };
    }
}