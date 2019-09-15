using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Containers.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Containers.Camunda.Authorization.Entities
{
    public class AuthorizationListItem : ICamundaEntity
    {
        public string Id { get; set; }
        public byte Type { get; set; }
        public IEnumerable<string> Permissions { get; set; }
        public string UserId { get; set; }
        public string GroupId { get; set; }
        public int ResourceType { get; set; }
        public string ResourceId { get; set; }
    }
}