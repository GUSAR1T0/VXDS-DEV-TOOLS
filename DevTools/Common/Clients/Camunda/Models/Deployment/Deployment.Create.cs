using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Entities;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.Deployment
{
    public static partial class Deployment
    {
        public class CreateRequest : CamundaRequest<CreateResponse>
        {
            public override CamundaAction Action => CamundaAction.DeploymentCreate;

            public string DeploymentName { get; set; }
            public string DeploymentSource { get; set; }
            public bool EnableDuplicateFiltering { get; set; }
            public bool DeployChangedOnly { get; set; }

            [HttpFileParameter]
            public IReadOnlyList<LocalFile> Files { get; set; }
        }

        public class CreateResponse : CamundaSingleResponse<DeploymentCreationResult>
        {
        }
    }
}