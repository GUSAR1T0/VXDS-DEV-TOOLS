using System.Collections.Generic;
using VXDesign.Store.DevTools.Core.Entities.Camunda.Base;
using VXDesign.Store.DevTools.Core.Entities.HTTP;

namespace VXDesign.Store.DevTools.SRS.Camunda
{
    public class CamundaRequest : IRequest
    {
        public CamundaEndpoint Endpoint { get; set; }
        public Dictionary<string, string> Path { get; set; }
        public Dictionary<string, string> Query { get; set; }
        public string Body { get; set; }
    }
}