using VXDesign.Store.DevTools.Common.Containers.Camunda.Base;
using VXDesign.Store.DevTools.SRS.Camunda;
using VXDesign.Store.DevTools.SRS.Syrinx.Models;
using CamundaRequestModel = VXDesign.Store.DevTools.SRS.Syrinx.Models.CamundaRequestModel;

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