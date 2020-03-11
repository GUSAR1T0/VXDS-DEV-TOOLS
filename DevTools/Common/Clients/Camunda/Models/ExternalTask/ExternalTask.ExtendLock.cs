using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.ExternalTask
{
    public static partial class ExternalTask
    {
        public class ExtendLockRequest : CamundaRequest<ExtendLockResponse>
        {
            public override CamundaAction Action => CamundaAction.ExternalTaskExtendLock;

            public readonly string id;

            public ExtendLockRequest(string id)
            {
                this.id = id;
            }

            public string WorkerId { get; set; }
            public int NewDuration { get; set; }
        }

        public class ExtendLockResponse : CamundaEmptyResponse
        {
        }
    }
}