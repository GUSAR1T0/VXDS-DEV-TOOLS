using VXDesign.Store.DevTools.Core.Entities.Camunda.Base;
using VXDesign.Store.DevTools.Core.Entities.Camunda.ExternalTask.Containers;
using VXDesign.Store.DevTools.Core.Enums.Camunda;

namespace VXDesign.Store.DevTools.Core.Entities.Camunda.ExternalTask.Models
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