using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.ProcessInstance
{
    public static partial class ProcessInstance
    {
        public class DeleteRequest : CamundaRequest<DeleteResponse>
        {
            public override CamundaAction Action => CamundaAction.ProcessInstanceDelete;

            public readonly string id;

            public DeleteRequest(string id)
            {
                this.id = id;
            }

            [HttpQueryParameter]
            public bool? FailIfNotExists { get; set; }
        }

        public class DeleteResponse : CamundaEmptyResponse
        {
        }
    }
}