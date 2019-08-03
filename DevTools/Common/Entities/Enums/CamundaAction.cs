using VXDesign.Store.DevTools.Common.Attributes.Camunda;

namespace VXDesign.Store.DevTools.Common.Entities.Enums
{
    public enum CamundaAction
    {
        #region Authorization

        [CamundaAction(CamundaCategory.Authorization, "Get List", HttpMethod.Get)]
        AuthorizationGetList = 101,

        [CamundaAction(CamundaCategory.Authorization, "Get List Count", HttpMethod.Get, "count")]
        AuthorizationGetListCount = 102

        #endregion
    }
}