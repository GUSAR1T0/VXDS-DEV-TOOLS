using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Core.Entities.Exceptions;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Enums.Operations;
using VXDesign.Store.DevTools.Core.Services.Operations;
using VXDesign.Store.DevTools.Core.Utils.Authentication;
using VXDesign.Store.DevTools.Core.Utils.Controllers;

namespace VXDesign.Store.DevTools.Core.Entities.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        private readonly IOperationService operationService;

        #region User Claims

        // null => GUEST / UNAUTHORIZED USER
        protected int? UserId => AuthenticationUtils.GetUserId(User.Claims);
        protected UserPermission UserPermissions => AuthenticationUtils.GetUserPermissions(User.Claims);

        #endregion

        protected BaseApiController(IOperationService operationService)
        {
            this.operationService = operationService;
        }

        protected ActionResult Execute(Func<OperationContext.OperationContextBuilder, OperationContext.OperationContextBuilder> builder, Action<IOperation> action)
        {
            Execute(builder, async operation =>
            {
                await Task.Run(() => action(operation));
                return true;
            }).Wait();
            return Ok();
        }

        protected ActionResult<T> Execute<T>(Func<OperationContext.OperationContextBuilder, OperationContext.OperationContextBuilder> builder, Func<IOperation, T> action)
        {
            return Execute(builder, async operation => await Task.Run(() => action(operation))).Result;
        }

        protected async Task<ActionResult> Execute(Func<OperationContext.OperationContextBuilder, OperationContext.OperationContextBuilder> builder, Func<IOperation, Task> action)
        {
            await Execute(builder, async operation =>
            {
                await action(operation);
                return true;
            });
            return Ok();
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
                return ApiControllerUtils.HandleException(BadRequest, e);
            }
            catch (NotFoundException e)
            {
                return ApiControllerUtils.HandleException(NotFound, e);
            }
            catch (AuthenticationException e) when (e.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return ApiControllerUtils.HandleException(Unauthorized, e);
            }
            catch (AuthenticationException e) when (e.StatusCode == StatusCodes.Status403Forbidden)
            {
                return ApiControllerUtils.HandleException(ApiControllerUtils.Forbidden, e);
            }
            catch (OperationException e)
            {
                return ApiControllerUtils.HandleException(BadRequest, e);
            }
            catch (Exception e)
            {
                return ApiControllerUtils.HandleException(ApiControllerUtils.InternalServerError, e);
            }
        }
    }
}