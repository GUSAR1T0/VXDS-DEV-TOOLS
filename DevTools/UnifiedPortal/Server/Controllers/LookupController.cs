using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Core.Entities.Controllers;
using VXDesign.Store.DevTools.Core.Entities.Properties;
using VXDesign.Store.DevTools.Core.Enums.Operations;
using VXDesign.Store.DevTools.Core.Services.Operations;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Lookup;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Properties;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Controllers
{
    [Route("api/[controller]")]
    public class LookupController : BaseApiController
    {
        private readonly SyrinxProperties syrinxProperties;

        public LookupController(IOperationService operationService, PortalProperties properties) : base(operationService)
        {
            syrinxProperties = properties.SyrinxProperties;
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
                        Host = syrinxProperties.Host,
                        Api = syrinxProperties.Api
                    }
                },
                LookupValues = new LookupValuesModel
                {
                    PortalPermissions = EnumModel.GetEnumModelValues<PortalPermission>()
                }
            });
        }
    }
}