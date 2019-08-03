using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Entities.Properties;
using VXDesign.Store.DevTools.Common.Utils.Properties;

namespace VXDesign.Store.DevTools.Common.Extensions
{
    public static class AspNetCoreExtensions
    {
        public static void SetupProperties<T>(this IServiceCollection services, IConfiguration configuration) where T : PropertiesMarker, new()
        {
            services.AddSingleton(factory => PropertiesCreator.Create<T>(configuration));
        }

        public static void SetupApiPath(this IApplicationBuilder app)
        {
            app.Map(new PathString("/api"), builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    var msg = JsonConvert.SerializeObject(new { Message = "Invalid request path" });
                    await context.Response.WriteAsync(msg).ConfigureAwait(false);
                });
            });
        }
    }
}