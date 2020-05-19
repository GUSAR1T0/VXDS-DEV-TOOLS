using System;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Entities
{
    public class DeploymentCreationResult : ICamundaEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string TenantId { get; set; }
        public DateTime DeploymentTime { get; set; }
    }
}