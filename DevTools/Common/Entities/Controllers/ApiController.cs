using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Models.Authorization;

namespace VXDesign.Store.DevTools.Common.Entities.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        protected ActionResult<T> HandleExceptionIfThrown<T>(Func<T> action)
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

        protected async Task<ActionResult<T>> HandleExceptionIfThrown<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (BadRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}