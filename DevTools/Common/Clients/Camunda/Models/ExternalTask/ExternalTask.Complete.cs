using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.ExternalTask
{
    public static partial class ExternalTask
    {
        public class CompleteRequest : CamundaRequest<CompleteResponse>
        {
            public override CamundaAction Action => CamundaAction.ExternalTaskComplete;

            public readonly string id;

            public CompleteRequest(string id)
            {
                this.id = id;
            }

            public string WorkerId { get; set; }
            public IReadOnlyCamundaVariables Variables { get; set; }
        }

        public class CompleteResponse : CamundaEmptyResponse
        {
        }
    }
}