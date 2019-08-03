using VXDesign.Store.DevTools.Common.Entities.Camunda;
using VXDesign.Store.DevTools.Common.Extensions.Enums;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Models.Camunda
{
    public class CamundaEndpointModel
    {
        public int Code { get; private set; }
        public string Category { get; private set; }
        public string Action { get; private set; }
        public string Endpoint { get; private set; }

        internal static CamundaEndpointModel Transform(CamundaEndpoint endpoint) => new CamundaEndpointModel
        {
            Code = endpoint.ActionCode,
            Category = endpoint.CategoryName,
            Action = endpoint.ActionName,
            Endpoint = $"{endpoint.Method.GetDescription()} {endpoint.Path}"
        };
    }
}