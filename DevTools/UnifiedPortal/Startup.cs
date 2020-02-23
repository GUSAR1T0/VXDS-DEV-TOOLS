using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VXDesign.Store.DevTools.Core.Extensions.Controllers;
using VXDesign.Store.DevTools.Core.Services.Operations;
using VXDesign.Store.DevTools.Core.Services.Storage;
using VXDesign.Store.DevTools.Core.Services.Syrinx;
using VXDesign.Store.DevTools.Core.Storage.DataStores;
using VXDesign.Store.DevTools.Core.Storage.LogStores;
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
            const string scope = "VXDS_UP";

            // Portal properties
            services.SetupProperties<PortalProperties>(Configuration);

            // Stores
            services.AddSingleton<ILoggerStore>(factory =>
            {
                var logStoreConnectionString = factory.GetService<PortalProperties>().DatabaseConnectionProperties.LogStoreConnectionString;
                return new LoggerStore(logStoreConnectionString, scope);
            });
            services.AddScoped<IUserDataStore, UserDataStore>();
            services.AddScoped<IUserRoleStore, UserRoleStore>();
            services.AddScoped<IDashboardStore, DashboardStore>();
            services.AddScoped<IOperationStore, OperationStore>();
            services.AddScoped<IPortalSettingsStore, PortalSettingsStore>();

            // Services
            services.AddScoped<IOperationService>(factory =>
            {
                var loggerStore = factory.GetService<ILoggerStore>();
                var dataStoreConnectionString = factory.GetService<PortalProperties>().DatabaseConnectionProperties.DataStoreConnectionString;
                return new OperationService(loggerStore, dataStoreConnectionString, scope);
            });
            services.AddScoped<ISyrinxCamundaClientService>(factory => new SyrinxCamundaClientService(factory.GetService<PortalProperties>().SyrinxProperties));
            services.AddScoped<ISyrinxAuthenticationClientService>(factory => new SyrinxAuthenticationClientService(factory.GetService<PortalProperties>().SyrinxProperties));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IOperationWithLogsService, OperationWithLogsService>();
            services.AddScoped<IPortalSettingsService, PortalSettingsService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddRouting(options => options.LowercaseUrls = true);
            services.ConfigureSwaggerDocument("1.0", "Unified Portal");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"));

            app.SetupApiPath();

            app.Run(async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            });
        }
    }
}