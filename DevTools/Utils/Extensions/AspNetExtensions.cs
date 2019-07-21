using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace VXDesign.Store.DevTools.Common.Extensions
{
    public static class AspNetCoreExtensions
    {
        public static void SetupApiPath(this IApplicationBuilder app)
        {
            app.Map(new PathString("/api"), builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    var msg = JsonConvert.SerializeObject(new {Message = "Invalid request path"});
                    await context.Response.WriteAsync(msg).ConfigureAwait(false);
                });
            });
        }
    }
}