using VXDesign.Store.DevTools.Core.Entities.Camunda.Base;
using VXDesign.Store.DevTools.Core.Enums.Camunda;

namespace VXDesign.Store.DevTools.Core.Entities.Camunda.ExternalTask.Models
{
    public static partial class ExternalTask
    {
        public class CompleteRequest : CamundaRequest<CompleteResponse>
        {
            public override CamundaAction Action => CamundaAction.ExternalTaskComplete;

            public readonly string id;

            public CompleteRequest(string id)
            {
                this.id = id;
            }

            public string WorkerId { get; set; }
            public IReadOnlyCamundaVariables Variables { get; set; }
        }

        public class CompleteResponse : CamundaEmptyResponse
        {
        }
    }
}