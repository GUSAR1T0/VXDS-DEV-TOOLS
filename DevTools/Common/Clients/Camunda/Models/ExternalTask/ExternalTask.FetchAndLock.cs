using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Entities;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.ExternalTask
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