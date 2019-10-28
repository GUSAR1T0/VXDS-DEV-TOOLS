using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Enums.HTTP;

namespace VXDesign.Store.DevTools.Common.Enums.Camunda
{
    public enum CamundaAction
    {
        #region Authorization

        [CamundaAction(CamundaCategory.Authorization, "Get List", HttpMethod.Get)]
        AuthorizationGetList = 1001,

        [CamundaAction(CamundaCategory.Authorization, "Get List Count", HttpMethod.Get, "count")]
        AuthorizationGetListCount = 1002,

        #endregion

        #region External Task

        [CamundaAction(CamundaCategory.ExternalTask, "Fetch and Lock External Tasks", HttpMethod.Post, "fetchAndLock")]
        ExternalTaskFetchAndLock = 12001,

        [CamundaAction(CamundaCategory.ExternalTask, "Complete External Task", HttpMethod.Post, "{id}/complete")]
        ExternalTaskComplete = 12002,

        [CamundaAction(CamundaCategory.ExternalTask, "Handle External Task Failure", HttpMethod.Post, "{id}/failure")]
        ExternalTaskHandleFailure = 12003,

        [CamundaAction(CamundaCategory.ExternalTask, "Extend Lock on External Task", HttpMethod.Post, "{id}/extendLock")]
        ExternalTaskExtendLock = 12004,

        [CamundaAction(CamundaCategory.ExternalTask, "Unlock an External Task", HttpMethod.Post, "{id}/unlock")]
        ExternalTaskUnlock = 12005,

        #endregion

        #region Process Definition

        [CamundaAction(CamundaCategory.ProcessDefinition, "Start Process Instance (by id)", HttpMethod.Post, "{id}/start")]
        ProcessDefinitionStartProcessInstanceById = 24001,

        [CamundaAction(CamundaCategory.ProcessDefinition, "Start Process Instance (by key)", HttpMethod.Post, "key/{key}/start")]
        ProcessDefinitionStartProcessInstanceByKey = 24002,

        [CamundaAction(CamundaCategory.ProcessDefinition, "Start Process Instance (by key and tenant id)", HttpMethod.Post, "key/{key}/tenant-id/{tenant-id}/start")]
        ProcessDefinitionStartProcessInstanceByKeyAndTenantId = 24003

        #endregion
    }
}