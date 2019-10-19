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

        private int? UserId => AuthorizationUtils.GetUserId(User.Claims); // null => GUEST / UNAUTHORIZED USER

        protected ApiController(IOperationService operationService)
        {
            this.operationService = operationService;
        }

        private static ObjectResult InternalServerError(object value) => new ObjectResult(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        private static ObjectResult HandleException(Func<ResponseResult, ObjectResult> response, OperationException exception) => response(new ResponseResult
        {
            OperationId = exception.OperationId.ToString(),
            Message = exception.Message
        });

        private static ObjectResult HandleException(Func<ResponseResult, ObjectResult> response, Exception exception) => response(new ResponseResult
        {
            OperationId = "Non-operational incident",
            Message = exception.Message
        });

        protected async Task<ActionResult> Execute(Func<OperationContext.OperationContextBuilder, OperationContext.OperationContextBuilder> builder, Func<IOperation, Task> action)
        {
            try
            {
                var operationContext = builder(OperationContext.Builder()).SetUserId(UserId).Create();
                await operationService.Make(operationContext, action);
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
            catch (OperationException e)
            {
                return HandleException(BadRequest, e);
            }
            catch (Exception e)
            {
                return HandleException(InternalServerError, e);
            }
        }

        protected async Task<ActionResult<T>> Execute<T>(Func<OperationContext.OperationContextBuilder, OperationContext.OperationContextBuilder> builder, Func<IOperation, Task<T>> action)
        {
            try
            {
                var operationContext = builder(OperationContext.Builder()).SetUserId(UserId).Create();
                return await operationService.Make(operationContext, action);
            }
            catch (BadRequestException e)
            {
                return HandleException(BadRequest, e);
            }
            catch (NotFoundException e)
            {
                return HandleException(NotFound, e);
            }
            catch (OperationException e)
            {
                return HandleException(BadRequest, e);
            }
            catch (Exception e)
            {
                return HandleException(InternalServerError, e);
            }
        }
    }
}