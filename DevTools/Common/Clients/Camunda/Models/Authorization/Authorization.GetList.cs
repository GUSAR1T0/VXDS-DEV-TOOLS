using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Entities;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.Authorization
{
    public static partial class Authorization
    {
        public class GetListRequest : CamundaRequest<GetListResponse>
        {
            public override CamundaAction Action => CamundaAction.AuthorizationGetList;

            [HttpQueryParameter]
            public string Id { get; set; }

            [HttpQueryParameter]
            public byte? Type { get; set; }

            [HttpQueryParameter]
            public IEnumerable<string> UserIdIn { get; set; }

            [HttpQueryParameter]
            public IEnumerable<string> GroupIdIn { get; set; }

            [HttpQueryParameter]
            public byte? ResourceType { get; set; }

            [HttpQueryParameter]
            public string ResourceId { get; set; }

            [HttpQueryParameter]
            public string SortBy { get; set; } // resourceType or resourceId

            [HttpQueryParameter]
            public string SortOrder { get; set; } // asc or desc

            [HttpQueryParameter]
            public int? FirstResult { get; set; }

            [HttpQueryParameter]
            public int? MaxResults { get; set; }
        }

        public class GetListResponse : CamundaMultipleResponse<AuthorizationListItem>
        {
        }
    }
}