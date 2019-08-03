using System;
using Microsoft.AspNetCore.Mvc;

namespace VXDesign.Store.DevTools.Common.Entities.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        protected ActionResult<T> HandleExceptionIfThrown<T>(Func<ActionResult<T>> action)
        {
            try
            {
                return action();
            }
            catch (ApiControllerException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}