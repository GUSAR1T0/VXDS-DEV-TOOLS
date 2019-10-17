using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VXDesign.Store.DevTools.Common.Extensions.Controllers;
using VXDesign.Store.DevTools.Common.Services.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStores;
using VXDesign.Store.DevTools.SRS.Authorization;
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
            // Portal properties
            services.SetupProperties<PortalProperties>(Configuration);

            // Operations handler
            services.AddScoped<IOperationService>(factory => new OperationService(factory.GetService<PortalProperties>().DatabaseConnectionProperties));

            // Stores
            services.AddScoped<IUserDataStore>(factory => new UserDataStore(factory.GetService<PortalProperties>().DatabaseConnectionProperties));

            // Services
            services.AddScoped<ICamundaServerService>(factory => new CamundaServerService(factory.GetService<PortalProperties>().CamundaProperties));
            services.AddScoped<IAuthorizationService>(factory =>
            {
                var properties = factory.GetService<PortalProperties>().AuthorizationTokenProperties;
                var userDataStore = factory.GetService<IUserDataStore>();
                return new AuthorizationService(properties, userDataStore);
            });

            // Authorization mechanism (workaround)
            var authorizationService = services.BuildServiceProvider().GetService<IAuthorizationService>();
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

            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddRouting(options => options.LowercaseUrls = true);
            services.ConfigureSwaggerDocument("1.0", "Syrinx");
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(policyBuilder => policyBuilder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
            );

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            app.SetupApiPath();
        }
    }
}