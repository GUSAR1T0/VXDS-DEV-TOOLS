using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VXDesign.Store.DevTools.Common.Extensions.Controllers;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.Common.Services.Storage;
using VXDesign.Store.DevTools.Common.Services.Syrinx;
using VXDesign.Store.DevTools.Common.Storage.DataStores;
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
            // Portal properties
            services.SetupProperties<PortalProperties>(Configuration);

            // Operations handler
            services.AddScoped<IOperationService>(factory => new OperationService(factory.GetService<PortalProperties>().DatabaseConnectionProperties, "VXDS_UP"));

            // Stores
            services.AddScoped<IUserDataStore, UserDataStore>();
            services.AddScoped<IUserRoleStore, UserRoleStore>();
            services.AddScoped<IDashboardStore, DashboardStore>();

            // Services
            services.AddScoped<ISyrinxCamundaClientService>(factory => new SyrinxCamundaClientService(factory.GetService<PortalProperties>().SyrinxProperties));
            services.AddScoped<ISyrinxAuthenticationClientService>(factory => new SyrinxAuthenticationClientService(factory.GetService<PortalProperties>().SyrinxProperties));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IDashboardService, DashboardService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddRouting(options => options.LowercaseUrls = true);
            services.ConfigureSwaggerDocument("1.0", "Unified Portal");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseOpenApi();
                app.UseSwaggerUi3();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

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