using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Entities;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.Deployment
{
    public static partial class Deployment
    {
        public class GetListRequest : CamundaRequest<GetListResponse>
        {
            public override CamundaAction Action => CamundaAction.DeploymentGetList;

            [HttpQueryParameter]
            public string Id { get; set; }

            [HttpQueryParameter]
            public string Name { get; set; }

            [HttpQueryParameter]
            public string NameLike { get; set; }

            [HttpQueryParameter]
            public string Source { get; set; }

            [HttpQueryParameter]
            public string WithoutSource { get; set; }

            [HttpQueryParameter]
            public string SortBy { get; set; }

            [HttpQueryParameter]
            public string SortOrder { get; set; }
        }

        public class GetListResponse : CamundaMultipleResponse<DeploymentListItem>
        {
        }
    }
}