using VXDesign.Store.DevTools.Common.Containers.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Containers.Camunda.ProcessDefinition.Entities
{
    public class ProcessInstanceStartResult : ICamundaEntity
    {
        public string Id { get; set; }
        public string DefinitionId { get; set; }
        public string BusinessKey { get; set; }
        public string CaseInstanceId { get; set; }
        public string TenantId { get; set; }
        public bool Ended { get; set; }
        public bool Suspended { get; set; }
        public IReadOnlyCamundaVariables Variables { get; set; } = new CamundaVariables();
    }
}