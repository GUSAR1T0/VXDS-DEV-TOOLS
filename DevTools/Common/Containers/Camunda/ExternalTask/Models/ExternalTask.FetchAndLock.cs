using VXDesign.Store.DevTools.Common.Containers.Camunda.Base;
using VXDesign.Store.DevTools.Common.Containers.Camunda.ExternalTask.Entities;
using VXDesign.Store.DevTools.Common.Enums.Camunda;

namespace VXDesign.Store.DevTools.Common.Containers.Camunda.ExternalTask.Models
{
    public static partial class ExternalTask
    {
        public class FetchAndLockRequest : CamundaRequest<FetchAndLockResponse>
        {
            public override CamundaAction Action => CamundaAction.ExternalTaskFetchAndLock;

            public string WorkerId { get; set; }
            public int MaxTasks { get; set; }
            public bool? UsePriority { get; set; }
            public int? AsyncResponseTimeout { get; set; }
            public IReadOnlyCamundaTopics Topics { get; set; }
        }

        public class FetchAndLockResponse : CamundaMultipleResponse<LockedExternalTaskListItem>
        {
        }
    }
}