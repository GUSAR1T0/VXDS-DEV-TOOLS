using VXDesign.Store.DevTools.Common.Containers.Camunda.Base;
using VXDesign.Store.DevTools.Common.Containers.Camunda.ProcessDefinition.Entities;
using VXDesign.Store.DevTools.Common.Enums.Camunda;

namespace VXDesign.Store.DevTools.Common.Containers.Camunda.ProcessDefinition.Models
{
    public static partial class ProcessDefinition
    {
        private interface IStartProcessInstance
        {
            string BusinessKey { get; set; }
            bool WithVariablesInReturn { get; set; }
            ICamundaVariables Variables { get; set; }
        }

        private static class StartProcessInstanceRequestUtils
        {
            public static void SetVariablesWithErrorField(out ICamundaVariables refToRequestVariables, ICamundaVariables newVariables)
            {
                refToRequestVariables = newVariables;
                if (!refToRequestVariables.ContainsKey("error"))
                {
                    refToRequestVariables.Add("error", "");
                }
            }
        }

        public class StartProcessInstanceByIdRequest : CamundaRequest<StartProcessInstanceResponse>, IStartProcessInstance
        {
            public override CamundaAction Action => CamundaAction.ProcessDefinitionStartProcessInstanceById;

            public readonly string id;
            private ICamundaVariables variables;

            public StartProcessInstanceByIdRequest(string id)
            {
                this.id = id;
            }

            public string BusinessKey { get; set; }
            public bool WithVariablesInReturn { get; set; }

            public ICamundaVariables Variables
            {
                get => variables;
                set => StartProcessInstanceRequestUtils.SetVariablesWithErrorField(out variables, value);
            }
        }

        public class StartProcessInstanceByKeyRequest : CamundaRequest<StartProcessInstanceResponse>, IStartProcessInstance
        {
            public override CamundaAction Action => CamundaAction.ProcessDefinitionStartProcessInstanceByKey;

            public readonly string key;
            private ICamundaVariables variables;

            public StartProcessInstanceByKeyRequest(string key)
            {
                this.key = key;
            }

            public string BusinessKey { get; set; }
            public bool WithVariablesInReturn { get; set; }

            public ICamundaVariables Variables
            {
                get => variables;
                set => StartProcessInstanceRequestUtils.SetVariablesWithErrorField(out variables, value);
            }
        }

        public class StartProcessInstanceByKeyAndTenantIdRequest : CamundaRequest<StartProcessInstanceResponse>, IStartProcessInstance
        {
            public override CamundaAction Action => CamundaAction.ProcessDefinitionStartProcessInstanceByKeyAndTenantId;

            public readonly string key;
            public readonly string tenantId;
            private ICamundaVariables variables;

            public StartProcessInstanceByKeyAndTenantIdRequest(string key, string tenantId)
            {
                this.key = key;
                this.tenantId = tenantId;
            }

            public string BusinessKey { get; set; }
            public bool WithVariablesInReturn { get; set; }

            public ICamundaVariables Variables
            {
                get => variables;
                set => StartProcessInstanceRequestUtils.SetVariablesWithErrorField(out variables, value);
            }
        }

        public class StartProcessInstanceResponse : CamundaSingleResponse<ProcessInstanceStartResult>
        {
        }
    }
}