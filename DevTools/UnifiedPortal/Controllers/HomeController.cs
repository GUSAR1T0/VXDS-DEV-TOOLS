using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Containers.Properties;
using VXDesign.Store.DevTools.UnifiedPortal.Properties;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly SyrinxProperties syrinxProperties;

        public HomeController(PortalProperties properties)
        {
            syrinxProperties = properties.SyrinxProperties;
        }

        /// <summary>
        /// Loads the Web application
        /// </summary>
        /// <returns>Prepared content of the application</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }

        /// <summary>
        /// Sends "bad request" response
        /// </summary>
        /// <returns>Error with message</returns>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("Error")]
        public IActionResult Error()
        {
            return BadRequest("Error page");
        }

        /// <summary>
        /// Returns a dictionary of environment values which can be used by frontend
        /// </summary>
        /// <returns>Dictionary with values</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("env")]
        public Dictionary<string, string> GetEnvironmentVariables() => new Dictionary<string, string>
        {
            // TODO: should be like as a part of lookup
            { "LOCALHOST_API", "api" },
            { "SYRINX_HOST", syrinxProperties.Host },
            { "SYRINX_API", syrinxProperties.Api }
        };
    }
}