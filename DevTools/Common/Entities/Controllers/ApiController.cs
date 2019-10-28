using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Enums.Operations;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.Common.Utils.Authentication;
using VXDesign.Store.DevTools.Common.Utils.Controllers;

namespace VXDesign.Store.DevTools.Common.Entities.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        private readonly IOperationService operationService;

        #region User Claims

        // null => GUEST / UNAUTHORIZED USER
        protected int? UserId => AuthenticationUtils.GetUserId(User.Claims);
        protected UserPermission UserPermissions => AuthenticationUtils.GetUserPermissions(User.Claims);

        #endregion

        protected ApiController(IOperationService operationService)
        {
            this.operationService = operationService;
        }

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