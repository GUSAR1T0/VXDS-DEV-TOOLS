using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Services.Operations;

namespace VXDesign.Store.DevTools.Common.Entities.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        private readonly IOperationService operationService;

        protected ApiController(IOperationService operationService)
        {
            this.operationService = operationService;
        }

        private static ObjectResult InternalServerError(object value) => new ObjectResult(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        private static ObjectResult HandleException(Func<ResponseResult, ObjectResult> response, Exception exception) => response(new ResponseResult
        {
            Message = exception.Message
        });

        protected async Task<ActionResult> Execute(Func<IOperation, Task> action)
        {
            try
            {
                await operationService.Make(action);
                return Ok();
            }
            catch (BadRequestException e)
            {
                return HandleException(BadRequest, e);
            }
            catch (NotFoundException e)
            {
                return HandleException(NotFound, e);
            }
            catch (Exception e)
            {
                return HandleException(InternalServerError, e);
            }
        }

        protected async Task<ActionResult<T>> Execute<T>(Func<IOperation, Task<T>> action)
        {
            try
            {
                return await operationService.Make(action);
            }
            catch (BadRequestException e)
            {
                return HandleException(BadRequest, e);
            }
            catch (NotFoundException e)
            {
                return HandleException(NotFound, e);
            }
            catch (Exception e)
            {
                return HandleException(InternalServerError, e);
            }
        }
    }
}