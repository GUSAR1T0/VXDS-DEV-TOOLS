using VXDesign.Store.DevTools.Common.Containers.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Containers.Camunda.Authorization.Entities
{
    public class AuthorizationListCount : ICamundaEntity
    {
        public int Count { get; set; }
    }
}