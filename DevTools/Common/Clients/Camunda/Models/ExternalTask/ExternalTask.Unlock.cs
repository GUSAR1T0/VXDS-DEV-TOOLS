using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.ExternalTask
{
    public static partial class ExternalTask
    {
        public class UnlockRequest : CamundaRequest<UnlockResponse>
        {
            public override CamundaAction Action => CamundaAction.ExternalTaskUnlock;

            public readonly string id;

            public UnlockRequest(string id)
            {
                this.id = id;
            }
        }

        public class UnlockResponse : CamundaEmptyResponse
        {
        }
    }
}