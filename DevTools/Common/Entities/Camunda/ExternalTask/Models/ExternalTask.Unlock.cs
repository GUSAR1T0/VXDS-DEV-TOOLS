using VXDesign.Store.DevTools.Common.Entities.Camunda.Base;
using VXDesign.Store.DevTools.Common.Enums.Camunda;

namespace VXDesign.Store.DevTools.Common.Entities.Camunda.ExternalTask.Models
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