using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Entities
{
    public class AuthorizationListCount : ICamundaEntity
    {
        public int Count { get; set; }
    }
}