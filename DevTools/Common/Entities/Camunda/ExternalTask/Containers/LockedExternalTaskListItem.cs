using VXDesign.Store.DevTools.Common.Entities.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Entities.Camunda.ExternalTask.Containers
{
    public class LockedExternalTaskListItem : ICamundaEntity
    {
        public string ActivityId { get; set; }
        public string ActivityInstanceId { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }
        public string ExecutionId { get; set; }
        public string Id { get; set; }
        public string LockExpirationTime { get; set; }
        public string ProcessDefinitionId { get; set; }
        public string ProcessDefinitionKey { get; set; }
        public string ProcessInstanceId { get; set; }
        public string TenantId { get; set; }
        public int? Retries { get; set; }
        public string WorkerId { get; set; }
        public int Priority { get; set; }
        public string BusinessKey { get; set; }
        public IReadOnlyCamundaVariables Variables { get; set; } = new CamundaVariables();
    }
}