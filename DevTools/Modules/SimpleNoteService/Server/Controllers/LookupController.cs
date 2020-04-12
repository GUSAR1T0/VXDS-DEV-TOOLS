using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Authentication;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Constants;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.Lookup;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Properties;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Controllers
{
    [Route("api/[controller]")]
    public class LookupController : BaseApiController
    {
        private readonly PortalProperties portalProperties;

        public LookupController(IOperationService operationService, PortalProperties portalProperties) : base(operationService)
        {
            this.portalProperties = portalProperties;
        }

        /// <summary>
        /// Returns a dictionary of values which can be used by frontend
        /// </summary>
        /// <returns>Dictionary with values</returns>
        [ProducesResponseType(typeof(LookupModel), StatusCodes.Status200OK)]
        [HttpGet("values")]
        public ActionResult<LookupModel> GetAllValues()
        {
            return Execute(_ => new LookupModel
            {
                EnvironmentVariables = new EnvironmentVariablesModel
                {
                    Syrinx = new SyrinxHostModel
                    {
                        Host = portalProperties.SyrinxProperties.Host,
                        Api = portalProperties.SyrinxProperties.Api
                    },
                    UnifiedPortal = new UnifiedPortalHostModel
                    {
                        Host = portalProperties.UnifiedPortalProperties.Host,
                        Api = portalProperties.UnifiedPortalProperties.Api
                    }
                },
                LookupValues = new LookupValuesModel
                {
                    PermissionGroupId = PortalPermissionKey.PermissionGroupId,
                    PortalPermissions = EnumModel.GetEnumModelValues<PortalPermission>()
                }
            });
        }
    }
}