using VXDesign.Store.DevTools.Core.Entities.Camunda.Base;

namespace VXDesign.Store.DevTools.Core.Entities.Camunda.Authorization.Containers
{
    public class AuthorizationListCount : ICamundaEntity
    {
        public int Count { get; set; }
    }
}