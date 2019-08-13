using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Entities.Properties;
using VXDesign.Store.DevTools.Common.Utils.Authorization;
using VXDesign.Store.DevTools.Common.Utils.Properties;

namespace VXDesign.Store.DevTools.Common.Extensions.Controllers
{
    public static class AspNetCoreExtensions
    {
        public static void SetupAuthentication(this IServiceCollection services, AuthorizationTokenProperties authorizationTokenProperties)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = AuthorizationService.GetServerTokenValidationParameters(authorizationTokenProperties);
            });
        }

        public static T SetupProperties<T>(this IServiceCollection services, IConfiguration configuration) where T : class, IPropertiesMarker, new()
        {
            var properties = PropertiesCreator.Create<T>(configuration);
            services.AddSingleton(factory => properties);
            return properties;
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