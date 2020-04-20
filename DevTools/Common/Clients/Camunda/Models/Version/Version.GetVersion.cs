using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Entities;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Models.Version
{
    public static partial class Version
    {
        public class GetVersionRequest : CamundaRequest<GetVersionResponse>
        {
            public override CamundaAction Action => CamundaAction.Version;
        }

        public class GetVersionResponse : CamundaSingleResponse<VersionResult>
        {
        }
    }
}