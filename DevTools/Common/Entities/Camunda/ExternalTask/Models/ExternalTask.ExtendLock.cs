using VXDesign.Store.DevTools.Common.Entities.Camunda.Base;
using VXDesign.Store.DevTools.Common.Enums.Camunda;

namespace VXDesign.Store.DevTools.Common.Entities.Camunda.ExternalTask.Models
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