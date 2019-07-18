﻿using Microsoft.AspNetCore.Mvc;

namespace VXDesign.Store.DevTools.UnifiedPortal.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }

        [HttpGet("Error")]
        public IActionResult Error()
        {
            return BadRequest("Error page");
        }
    }
}