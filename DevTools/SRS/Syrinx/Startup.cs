using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VXDesign.Store.DevTools.Core.Extensions.Controllers;
using VXDesign.Store.DevTools.Core.Services.Operations;
using VXDesign.Store.DevTools.Core.Storage.DataStores;
using VXDesign.Store.DevTools.Core.Storage.LogStores;
using VXDesign.Store.DevTools.SRS.Authentication;
using VXDesign.Store.DevTools.SRS.Camunda;
using VXDesign.Store.DevTools.SRS.Syrinx.Properties;

namespace VXDesign.Store.DevTools.SRS.Syrinx
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
            const string scope = "VXDS_SRS";

            // Portal properties
            services.SetupProperties<PortalProperties>(Configuration);

            // Stores
            services.AddSingleton<ILoggerStore>(factory =>
            {
                var logStoreConnectionString = factory.GetService<PortalProperties>().DatabaseConnectionProperties.LogStoreConnectionString;
                return new LoggerStore(logStoreConnectionString, scope);
            });
            services.AddScoped<IUserDataStore, UserDataStore>();

            // Services
            services.AddScoped<IOperationService>(factory =>
            {
                var loggerStore = factory.GetService<ILoggerStore>();
                var dataStoreConnectionString = factory.GetService<PortalProperties>().DatabaseConnectionProperties.DataStoreConnectionString;
                return new OperationService(loggerStore, dataStoreConnectionString, scope);
            });
            services.AddScoped<ICamundaServerService>(factory => new CamundaServerService(factory.GetService<PortalProperties>().CamundaProperties));
            services.AddScoped<IAuthenticationService>(factory =>
            {
                var properties = factory.GetService<PortalProperties>().AuthenticationTokenProperties;
                var userDataStore = factory.GetService<IUserDataStore>();
                return new AuthenticationService(properties, userDataStore);
            });

            // Authorization mechanism (workaround)
            var authorizationService = services.BuildServiceProvider().GetService<IAuthenticationService>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = authorizationService.GetServerTokenValidationParameters();
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddRouting(options => options.LowercaseUrls = true);
            services.ConfigureSwaggerDocument("1.0", "Syrinx");
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRouting();
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.SetupApiPath();
        }
    }
}