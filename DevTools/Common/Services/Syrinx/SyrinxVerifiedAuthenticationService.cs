using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Storage;
using VXDesign.Store.DevTools.Common.Entities.Syrinx;
using VXDesign.Store.DevTools.Common.Extensions.HTTP;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.Common.Utils.Authentication;
using VXDesign.Store.DevTools.Common.Utils.Controllers;

namespace VXDesign.Store.DevTools.Common.Services.Syrinx
{
    public class SyrinxVerifiedAuthenticationService : IAsyncAuthorizationFilter
    {
        private readonly ISyrinxAuthenticationClientService service;
        private readonly IOperationService operationService;
        private readonly Permissions permissions;

        public SyrinxVerifiedAuthenticationService(ISyrinxAuthenticationClientService service, IOperationService operationService, Permissions permissions)
        {
            this.service = service;
            this.operationService = operationService;
            this.permissions = permissions;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var operationContext = OperationContext.Builder()
                .SetName(nameof(SyrinxVerifiedAuthenticationService), "OnAuthorization")
                .SetUserId(null, true)
                .Create();
            try
            {
                await operationService.Make(operationContext, async operation => await ValidateToken(context, operation));
            }
            catch (AuthenticationException e) when (e.StatusCode == StatusCodes.Status401Unauthorized)
            {
                context.Result = ApiControllerUtils.HandleException(result => new UnauthorizedObjectResult(result), e);
            }
            catch (AuthenticationException e) when (e.StatusCode == StatusCodes.Status403Forbidden)
            {
                context.Result = ApiControllerUtils.HandleException(ApiControllerUtils.Forbidden, e);
            }
            catch (Exception e)
            {
                context.Result = ApiControllerUtils.HandleException(ApiControllerUtils.InternalServerError, e);
            }
        }

        private async Task ValidateToken(AuthorizationFilterContext context, IOperation operation)
        {
            var token = context.HttpContext.Request?.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                var result = await service.Send(operation, new VerifyAuthenticationRequest
                {
                    Token = token
                });

                if (result.IsWithoutErrors() && !string.IsNullOrWhiteSpace(result.Output))
                {
                    var syrinxAuthorizationClaims = JsonConvert.DeserializeObject<UserAuthorizationEntity>(result.Output);

                    if (!permissions.HasPermissions(syrinxAuthorizationClaims))
                    {
                        throw CommonExceptions.AccessDenied(operation, StatusCodes.Status403Forbidden);
                    }

                    context.HttpContext.User.AddIdentity(AuthenticationUtils.GetClaimsIdentity(syrinxAuthorizationClaims));
                    return;
                }
            }

            throw CommonExceptions.AccessDenied(operation, StatusCodes.Status401Unauthorized);
        }
    }
}