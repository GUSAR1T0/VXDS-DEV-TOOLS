using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Controllers;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.Entities.Incident;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Entities.Notification;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Core.Properties;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Authentication;
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
        [ProducesResponseType(typeof(ResponseResult), StatusCodes.Status400BadRequest)]
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
                    PortalPermissions = EnumModel.GetEnumModelValues<PortalPermission>(),
                    IncidentStatuses = EnumModel.GetEnumModelValues<IncidentStatus>(),
                    NotificationLevels = EnumModel.GetEnumModelValues<NotificationLevel>(),
                    HostOperatingSystems = EnumModel.GetEnumModelValues<HostOperatingSystem>(),
                    HostConnectionTypes = EnumModel.GetEnumModelValues<HostConnectionType>(),
                    ModuleStatuses = EnumModel.GetEnumModelValues<ModuleStatus>(),
                    ModuleConfigurationVerdicts = EnumModel.GetEnumModelValues<ModuleConfigurationVerdict>(),
                    FileExtensions = EnumModel.GetEnumModelValues<FileExtension>()
                }
            });
        }
    }
}