using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Entities.Properties;
using VXDesign.Store.DevTools.Common.Services.Authorization;
using VXDesign.Store.DevTools.Common.Utils.Properties;

namespace VXDesign.Store.DevTools.Common.Extensions.Controllers
{
    public static class AspNetCoreExtensions
    {
        #region Functions to add services

        private static T AddService<T>(Func<T> serviceCreator, Func<Func<IServiceProvider, T>, IServiceCollection> addToCollectionFunction) where T : class
        {
            var service = serviceCreator();
            addToCollectionFunction(factory => service);
            return service;
        }

        public static T AddSingletonService<T>(this IServiceCollection services, Func<T> serviceCreator) where T : class => AddService(serviceCreator, services.AddSingleton);

        public static T AddScopedService<T>(this IServiceCollection services, Func<T> serviceCreator) where T : class => AddService(serviceCreator, services.AddScoped);

        public static T AddTransientService<T>(this IServiceCollection services, Func<T> serviceCreator) where T : class => AddService(serviceCreator, services.AddTransient);

        #endregion

        public static TProperties SetupProperties<TProperties>(this IServiceCollection services, IConfiguration configuration) where TProperties : class, IPropertiesMarker, new()
        {
            return services.AddSingletonService(() => PropertiesCreator.Create<TProperties>(configuration));
        }

        public static void SetupAuthentication(this IServiceCollection services, IAuthorizationService authorizationService)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = authorizationService.GetServerTokenValidationParameters();
            });
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