using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            return BadRequest("Error page");
        }
    }
}