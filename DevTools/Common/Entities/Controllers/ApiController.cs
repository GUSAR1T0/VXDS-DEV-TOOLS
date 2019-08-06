using System;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;

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
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}