using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.Deployment
{
    public static partial class Deployment
    {
        public class DeleteRequest : CamundaRequest<DeleteResponse>
        {
            public override CamundaAction Action => CamundaAction.DeploymentDelete;

            public readonly string id;

            public DeleteRequest(string id)
            {
                this.id = id;
            }

            [HttpQueryParameter]
            public bool? Cascade { get; set; }
        }

        public class DeleteResponse : CamundaEmptyResponse
        {
        }
    }
}