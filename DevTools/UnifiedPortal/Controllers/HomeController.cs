using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
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
    }
}