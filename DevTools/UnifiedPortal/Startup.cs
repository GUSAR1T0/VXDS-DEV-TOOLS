﻿using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VXDesign.Store.DevTools.Common.Extensions.Controllers;
using VXDesign.Store.DevTools.Common.Utils.Camunda;
using VXDesign.Store.DevTools.Common.Utils.DataStorage;
using VXDesign.Store.DevTools.UnifiedPortal.Properties;

namespace VXDesign.Store.DevTools.UnifiedPortal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var portalProperties = services.SetupProperties<PortalProperties>(Configuration);
            services.SetupAuthentication(portalProperties.AuthorizationTokenProperties);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddRouting(options => options.LowercaseUrls = true);
            services.ConfigureSwaggerDocument("1.0", "Unified Portal");
            services.AddScoped<ICamundaClientService>(factory => new CamundaClientService(portalProperties.SyrinxProperties));
            services.AddScoped<IUserDataService>(factory => new UserDataService(portalProperties.DatabaseConnectionProperties));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.SetupApiPath();

            app.Run(async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            });
        }
    }
}