using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.Common.Utils.Authorization;

namespace VXDesign.Store.DevTools.Common.Entities.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        private readonly IOperationService operationService;

        private int UserId => AuthorizationUtils.GetUserId(User.Claims) ?? -1; // -1 => GUEST / UNAUTHORIZED USER

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

        protected async Task<ActionResult> Execute(Func<OperationContext> context, Func<IOperation, Task> action)
        {
            try
            {
                await operationService.Make(UserId, context(), action);
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

        protected async Task<ActionResult<T>> Execute<T>(Func<OperationContext> context, Func<IOperation, Task<T>> action)
        {
            try
            {
                return await operationService.Make(UserId, context(), action);
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