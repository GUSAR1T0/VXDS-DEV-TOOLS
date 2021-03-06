using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.ExternalTask
{
    public static partial class ExternalTask
    {
        public class HandleFailureRequest : CamundaRequest<HandleFailureResponse>
        {
            public override CamundaAction Action => CamundaAction.ExternalTaskHandleFailure;

            public readonly string id;

            public HandleFailureRequest(string id)
            {
                this.id = id;
            }

            public string WorkerId { get; set; }
            public string ErrorMessage { get; set; }
            public string ErrorDetails { get; set; }
            public int Retries { get; set; }
            public int RetryTimeout { get; set; }
        }

        public class HandleFailureResponse : CamundaEmptyResponse
        {
        }
    }
}