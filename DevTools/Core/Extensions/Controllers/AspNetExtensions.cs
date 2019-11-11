using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Web;
using VXDesign.Store.DevTools.Core.Entities.Properties;
using VXDesign.Store.DevTools.Core.Utils.Base;
using VXDesign.Store.DevTools.Core.Utils.Properties;

namespace VXDesign.Store.DevTools.Core.Extensions.Controllers
{
    public static class AspNetCoreExtensions
    {
        #region Web host configuration

        public static IWebHostBuilder UseNLogExtension(this IWebHostBuilder builder) => builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .UseNLog();

        public static void LaunchWebApplication(this IWebHost webHost) => ConsoleApplicationLauncher.Launch(webHost.Run);

        #endregion

        public static void SetupProperties<TProperties>(this IServiceCollection services, IConfiguration configuration) where TProperties : class, IPropertiesMarker, new()
        {
            services.AddSingleton(factory => PropertiesCreator.Create<TProperties>(configuration));
        }

        public static void SetupApiPath(this IApplicationBuilder app)
        {
            app.Map(new PathString("/api"), builder => builder.Run(async context =>
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                var msg = JsonConvert.SerializeObject(new { Message = "Invalid request path" });
                await context.Response.WriteAsync(msg).ConfigureAwait(false);
            }));
        }
    }
}