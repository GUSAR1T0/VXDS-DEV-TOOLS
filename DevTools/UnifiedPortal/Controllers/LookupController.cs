using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Entities.Properties;
using VXDesign.Store.DevTools.Common.Enums.Operations;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.UnifiedPortal.Models.Lookup;
using VXDesign.Store.DevTools.UnifiedPortal.Properties;
using VXDesign.Store.DevTools.UnifiedPortal.Utils;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("api/[controller]")]
    public class LookupController : ApiController
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
        public async Task<ActionResult<LookupModel>> GetAllValues()
        {
            return await Execute(OperationContexts.GetAllValues, async _ => new LookupModel
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
                    UserPermissions = EnumModel.GetEnumModelValues<UserPermission>()
                }
            });
        }
    }
}