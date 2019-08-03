using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Attributes.HTTP;
using VXDesign.Store.DevTools.Common.Entities.Camunda.Authorization;
using VXDesign.Store.DevTools.Common.Entities.Enums;
using VXDesign.Store.DevTools.Common.Models.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Models.Camunda.Authorization
{
    public class AuthorizationGetListRequestModel : CamundaRequestModel
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

    public class AuthorizationGetListResponseModel : CamundaMultipleResponseModel<AuthorizationList>
    {
    }
}