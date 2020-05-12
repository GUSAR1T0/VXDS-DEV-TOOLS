using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.ExternalTask
{
    public static partial class ExternalTask
    {
        public class HandleBpmnErrorRequest : CamundaRequest<HandleBpmnErrorResponse>
        {
            public override CamundaAction Action => CamundaAction.ExternalTaskHandleBpmnError;

            public readonly string id;

            public HandleBpmnErrorRequest(string id)
            {
                this.id = id;
            }

            public string WorkerId { get; set; }
            public string ErrorCode { get; set; }
            public string ErrorMessage { get; set; }
            public IReadOnlyCamundaVariables Variables { get; set; }
        }

        public class HandleBpmnErrorResponse : CamundaEmptyResponse
        {
        }
    }
}