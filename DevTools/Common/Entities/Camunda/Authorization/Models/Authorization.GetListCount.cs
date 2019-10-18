using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Camunda.Authorization.Containers;
using VXDesign.Store.DevTools.Common.Entities.Camunda.Base;
using VXDesign.Store.DevTools.Common.Enums.Camunda;

namespace VXDesign.Store.DevTools.Common.Entities.Camunda.Authorization.Models
{
    public static partial class Authorization
    {
        public class GetListCountRequest : CamundaRequest<GetListCountResponse>
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

        public class GetListCountResponse : CamundaSingleResponse<AuthorizationListCount>
        {
        }
    }
}