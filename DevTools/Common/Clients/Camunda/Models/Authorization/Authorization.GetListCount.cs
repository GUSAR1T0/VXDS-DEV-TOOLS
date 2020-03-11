using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Entities;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.Authorization
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