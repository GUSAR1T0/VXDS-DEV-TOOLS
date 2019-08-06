using System.Collections.Generic;

namespace VXDesign.Store.DevTools.Common.Entities.Camunda.Authorization
{
    public class AuthorizationList : ICamundaEntity
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