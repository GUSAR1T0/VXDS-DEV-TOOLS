using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Core.Entities.Exceptions;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Entities.Storage.User;
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
        protected IEnumerable<UserRolePermissionEntity> UserPermissions => AuthenticationUtils.GetUserPermissions(User.Claims);

        #endregion

        protected BaseApiController(IOperationService operationService)
        {
            this.operationService = operationService;
        }

        protected ActionResult Execute(Action<IOperation> action, [CallerMemberName] string callerName = "")
        {
            var result = Execute(async operation =>
            {
                await Task.Run(() => action(operation));
                return true;
            }, callerName).Result;
            return result.Value ? Ok() : result.Result;
        }

        protected ActionResult<T> Execute<T>(Func<IOperation, T> action, [CallerMemberName] string callerName = "")
        {
            return Execute(async operation => await Task.Run(() => action(operation)), callerName).Result;
        }

        protected async Task<ActionResult> Execute(Func<IOperation, Task> action, [CallerMemberName] string callerName = "")
        {
            var result = await Execute(async operation =>
            {
                await action(operation);
                return true;
            }, callerName);
            return result.Value ? Ok() : result.Result;
        }

        protected async Task<ActionResult<T>> Execute<T>(Func<IOperation, Task<T>> action, [CallerMemberName] string callerName = "")
        {
            try
            {
                var operationContext = OperationContext.Builder().SetName(GetType().FullName, callerName).SetUserId(UserId).Create();
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