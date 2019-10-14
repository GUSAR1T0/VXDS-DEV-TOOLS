using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;

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
                return BadRequest(new
                {
                    e.Message
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new
                {
                    e.Message
                });
            }
        }

        protected ActionResult HandleExceptionIfThrown(Action action)
        {
            try
            {
                action();
                return Ok();
            }
            catch (BadRequestException e)
            {
                return BadRequest(new
                {
                    e.Message
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new
                {
                    e.Message
                });
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
                return BadRequest(new
                {
                    e.Message
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new
                {
                    e.Message
                });
            }
        }

        protected async Task<ActionResult> HandleExceptionIfThrown(Func<Task> action)
        {
            try
            {
                await action();
                return Ok();
            }
            catch (BadRequestException e)
            {
                return BadRequest(new
                {
                    e.Message
                });
            }
            catch (NotFoundException e)
            {
                return NotFound(new
                {
                    e.Message
                });
            }
        }
    }
}