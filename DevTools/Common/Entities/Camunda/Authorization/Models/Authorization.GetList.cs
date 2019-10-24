using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Camunda.Authorization.Containers;
using VXDesign.Store.DevTools.Common.Entities.Camunda.Base;
using VXDesign.Store.DevTools.Common.Enums.Camunda;

namespace VXDesign.Store.DevTools.Common.Entities.Camunda.Authorization.Models
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