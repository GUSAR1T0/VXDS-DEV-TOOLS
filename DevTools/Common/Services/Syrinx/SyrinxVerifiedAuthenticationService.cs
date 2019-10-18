using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VXDesign.Store.DevTools.Common.Entities.Syrinx;
using VXDesign.Store.DevTools.Common.Extensions.HTTP;
using VXDesign.Store.DevTools.Common.Utils.Authorization;

namespace VXDesign.Store.DevTools.Common.Services.Syrinx
{
    public class SyrinxVerifiedAuthenticationService : IAsyncAuthorizationFilter
    {
        private readonly ISyrinxAuthenticationClientService service;

        public SyrinxVerifiedAuthenticationService(ISyrinxAuthenticationClientService service)
        {
            this.service = service;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request?.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                var result = await service.Send(new VerifyAuthenticationRequest
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
        }
    }
}