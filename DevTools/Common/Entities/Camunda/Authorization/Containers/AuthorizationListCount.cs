using VXDesign.Store.DevTools.Common.Entities.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Entities.Camunda.Authorization.Containers
{
    public class AuthorizationListCount : ICamundaEntity
    {
        public int Count { get; set; }
    }
}