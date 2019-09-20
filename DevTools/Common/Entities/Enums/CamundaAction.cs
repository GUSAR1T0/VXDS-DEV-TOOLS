using VXDesign.Store.DevTools.Common.Attributes;

namespace VXDesign.Store.DevTools.Common.Entities.Enums
{
    public enum CamundaAction
    {
        #region Authorization

        [CamundaAction(CamundaCategory.Authorization, "Get List", HttpMethod.Get)]
        AuthorizationGetList = 101,

        [CamundaAction(CamundaCategory.Authorization, "Get List Count", HttpMethod.Get, "count")]
        AuthorizationGetListCount = 102,

        #endregion

        #region External Task

        

        #endregion

        #region Process Definition

        // id

        [CamundaAction(CamundaCategory.ProcessDefinition, "Start Process Instance (by key)", HttpMethod.Post, "key/{key}/start")]
        ProcessDefinitionStartProcessInstance = 2402,

        // key + tenant id

        #endregion
    }
}