using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Containers.Camunda.Authorization.Entities;
using VXDesign.Store.DevTools.Common.Containers.Camunda.Base;
using VXDesign.Store.DevTools.Common.Entities.Enums;

namespace VXDesign.Store.DevTools.Common.Containers.Camunda.Authorization.Models
{
    public partial class Authorization
    {
        public class GetListRequestModel : CamundaRequestModel<GetListResponseModel>
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

        public class GetListResponseModel : CamundaMultipleResponseModel<AuthorizationListItem>
        {
        }
    }
}