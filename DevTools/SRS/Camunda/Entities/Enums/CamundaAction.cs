using VXDesign.Store.DevTools.Common.Entities.Enums;
using VXDesign.Store.DevTools.SRS.Camunda.Attributes;

namespace VXDesign.Store.DevTools.SRS.Camunda.Entities.Enums
{
    internal enum CamundaAction
    {
        #region Authorization

        [CamundaAction(CamundaCategory.Authorization, "Get List", HttpMethod.Get)]
        AuthorizationGetList = 101,

        [CamundaAction(CamundaCategory.Authorization, "Get List Count", HttpMethod.Get, "count")]
        AuthorizationGetListCount = 102,

        [CamundaAction(CamundaCategory.Authorization, "Get", HttpMethod.Get, "{id}")]
        AuthorizationGet = 103,

        [CamundaAction(CamundaCategory.Authorization, "Check", HttpMethod.Get, "check")]
        AuthorizationCheck = 104,

        [CamundaAction(CamundaCategory.Authorization, "Create", HttpMethod.Post, "create")]
        AuthorizationCreate = 105,

        [CamundaAction(CamundaCategory.Authorization, "Update", HttpMethod.Put, "{id}")]
        AuthorizationUpdate = 106,

        [CamundaAction(CamundaCategory.Authorization, "Delete", HttpMethod.Delete, "{id}")]
        AuthorizationDelete = 107

        #endregion
    }
}