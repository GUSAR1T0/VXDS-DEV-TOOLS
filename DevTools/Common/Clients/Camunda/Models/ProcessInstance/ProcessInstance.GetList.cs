using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Entities;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.ProcessInstance
{
    public static partial class ProcessInstance
    {
        public class GetListRequest : CamundaRequest<GetListResponse>
        {
            public override CamundaAction Action => CamundaAction.ProcessInstanceGetList;

            [HttpQueryParameter]
            public IEnumerable<string> ProcessInstanceIds { get; set; }

            [HttpQueryParameter]
            public string BusinessKey { get; set; }

            [HttpQueryParameter]
            public string BusinessKeyLike { get; set; }

            [HttpQueryParameter]
            public string ProcessDefinitionId { get; set; }

            [HttpQueryParameter]
            public string ProcessDefinitionKey { get; set; }

            [HttpQueryParameter]
            public IEnumerable<string> ProcessDefinitionKeyIn { get; set; }

            [HttpQueryParameter]
            public string SortBy { get; set; } // resourceType or resourceId

            [HttpQueryParameter]
            public string SortOrder { get; set; } // asc or desc

            [HttpQueryParameter]
            public int? FirstResult { get; set; }

            [HttpQueryParameter]
            public int? MaxResults { get; set; }
        }

        public class GetListResponse : CamundaMultipleResponse<ProcessInstanceListItem>
        {
        }
    }
}