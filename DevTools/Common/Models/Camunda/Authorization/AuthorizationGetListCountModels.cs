using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Attributes.HTTP;
using VXDesign.Store.DevTools.Common.Entities.Camunda.Authorization;
using VXDesign.Store.DevTools.Common.Entities.Enums;
using VXDesign.Store.DevTools.Common.Models.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Models.Camunda.Authorization
{
    public class AuthorizationGetListCountRequestModel : CamundaRequestModel
    {
        public override CamundaAction Action => CamundaAction.AuthorizationGetListCount;

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
    }

    public class AuthorizationGetListCountResponseModel : CamundaSingleResponseModel<AuthorizationListCount>
    {
    }
}