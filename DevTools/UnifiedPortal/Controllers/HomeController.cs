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
        [ProducesResponseType(200)]
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }

        /// <summary>
        /// Sends "bad request" response
        /// </summary>
        /// <returns>Error with message</returns>
        [ProducesResponseType(400)]
        [HttpGet("Error")]
        public IActionResult Error()
        {
            return BadRequest("Error page");
        }
    }
}