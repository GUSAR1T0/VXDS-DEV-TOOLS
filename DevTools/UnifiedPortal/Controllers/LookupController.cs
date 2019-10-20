using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Controllers;
using VXDesign.Store.DevTools.Common.Entities.Properties;
using VXDesign.Store.DevTools.Common.Enums.Operations;
using VXDesign.Store.DevTools.Common.Extensions.Base;
using VXDesign.Store.DevTools.Common.Services.Operations;
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
#pragma warning disable 1998
            return await Execute(OperationContexts.GetAllValues, async _ => new LookupModel
            {
                EnvironmentVariables = new EnvironmentVariables
                {
                    Syrinx = new
                    {
                        syrinxProperties.Host,
                        syrinxProperties.Api
                    }
                },
                LookupValues = new LookupValues
                {
                    UserPermissions = EnumExtensions.GetValues<UserPermission>().Select(x => new
                    {
                        Value = (int) x,
                        Name = x.GetDescription()
                    }),
                    UserRolePermissions = EnumExtensions.GetValues<UserRolePermission>().Select(x => new
                    {
                        Value = (int) x,
                        Name = x.GetDescription()
                    })
                }
            });
#pragma warning restore 1998
        }
    }

    public class EnvironmentVariables
    {
        public dynamic Syrinx { get; set; }
    }

    public class LookupValues
    {
        public IEnumerable<dynamic> UserRolePermissions { get; set; }
        public IEnumerable<dynamic> UserPermissions { get; set; }
    }

    public class LookupModel
    {
        public EnvironmentVariables EnvironmentVariables { get; set; }
        public LookupValues LookupValues { get; set; }
    }
}