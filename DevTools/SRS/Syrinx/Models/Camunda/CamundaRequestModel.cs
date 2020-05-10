using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Models.Camunda
{
    public class CamundaRequestModel
    {
        public CamundaAction Action { get; set; }
        public string Path { get; set; }
        public string Query { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}