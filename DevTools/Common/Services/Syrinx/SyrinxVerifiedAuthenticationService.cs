using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Syrinx;
using VXDesign.Store.DevTools.Common.Extensions.HTTP;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.Common.Utils.Authorization;

namespace VXDesign.Store.DevTools.Common.Services.Syrinx
{
    public class SyrinxVerifiedAuthenticationService : IAsyncAuthorizationFilter
    {
        private readonly ISyrinxAuthenticationClientService service;
        private readonly IOperationService operationService;

        public SyrinxVerifiedAuthenticationService(ISyrinxAuthenticationClientService service, IOperationService operationService)
        {
            this.service = service;
            this.operationService = operationService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            const int guestUserId = -1;
            var operationContext = OperationContext.Create(nameof(SyrinxVerifiedAuthenticationService), "OnAuthorization");

            try
            {
                await operationService.Make(guestUserId, operationContext, async operation =>
                {
                    var token = context.HttpContext.Request?.Headers["Authorization"];
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        var result = await service.Send(operation, new VerifyAuthenticationRequest
                        {
                            Token = token
                        });

                        if (result.IsWithoutErrors())
                        {
                            if (!string.IsNullOrWhiteSpace(result.Output))
                            {
                                context.HttpContext.User.AddIdentity(AuthorizationUtils.GetClaimsIdentity(result.Output));
                            }

                            return;
                        }
                    }

                    context.Result = new UnauthorizedResult();
                    throw CommonExceptions.AccessWasNotAccepted();
                });
            }
            catch
            {
                // ignored
            }
        }
    }
}