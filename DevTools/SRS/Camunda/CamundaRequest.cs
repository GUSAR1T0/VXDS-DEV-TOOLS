using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.SRS.Camunda
{
    public class CamundaRequest : IRequest
    {
        public CamundaEndpoint Endpoint { get; set; }
        public Dictionary<string, string> Path { get; set; }
        public Dictionary<string, string> Query { get; set; }
        public string Body { get; set; }
        public IReadOnlyList<LocalFile> Resources { get; set; }
    }
}