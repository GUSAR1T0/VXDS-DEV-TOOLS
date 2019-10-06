using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VXDesign.Store.DevTools.Common.Containers.Syrinx.VerifyAuthentication;
using VXDesign.Store.DevTools.Common.Extensions.HTTP;
using VXDesign.Store.DevTools.Common.Services.Syrinx;

namespace VXDesign.Store.DevTools.Common.Attributes
{
    public class SyrinxVerifiedAuthenticationAttribute : TypeFilterAttribute
    {
        public SyrinxVerifiedAuthenticationAttribute() : base(typeof(SyrinxVerifiedAuthenticationFilter))
        {
        }
    }

    public class SyrinxVerifiedAuthenticationFilter : IAsyncAuthorizationFilter
    {
        private readonly ISyrinxAuthenticationClientService service;

        public SyrinxVerifiedAuthenticationFilter(ISyrinxAuthenticationClientService service)
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

                if (result.IsWithoutErrors()) return;
            }

            context.Result = new UnauthorizedResult();
        }
    }
}