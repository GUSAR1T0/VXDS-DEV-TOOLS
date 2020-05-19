using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.SRS.Camunda;
using VXDesign.Store.DevTools.SRS.Syrinx.Models.Camunda;

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
            Path = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.Path),
            Query = JsonConvert.DeserializeObject<Dictionary<string, string>>(model.Query),
            Body = model.Body,
            Resources = model.Files?.Select((file, index) => new LocalFile
            {
                Name = $"file{index}",
                FileName = file.FileName,
                Stream = file.OpenReadStream()
            }).ToList() ?? new List<LocalFile>()
        };
    }
}