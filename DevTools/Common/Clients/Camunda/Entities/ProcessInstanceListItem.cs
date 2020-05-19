using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Entities
{
    public class ProcessInstanceListItem : ICamundaEntity
    {
        public string Id { get; set; }
        public string DefinitionId { get; set; }
        public string BusinessKey { get; set; }
        public string CaseInstanceId { get; set; }
        public bool Suspended { get; set; }
        public string TenantId { get; set; }
    }
}