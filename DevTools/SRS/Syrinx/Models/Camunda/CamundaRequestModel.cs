using System.Collections.Generic;
using VXDesign.Store.DevTools.Core.Enums.Camunda;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Models.Camunda
{
    public class CamundaRequestModel
    {
        public CamundaAction Action { get; set; }
        public Dictionary<string, string> Path { get; set; }
        public Dictionary<string, string> Query { get; set; }
        public string Body { get; set; }
    }
}