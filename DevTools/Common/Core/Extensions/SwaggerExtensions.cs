using Microsoft.Extensions.DependencyInjection;
using NSwag;

namespace VXDesign.Store.DevTools.Common.Core.Extensions
{
    public static class SwaggerExtensions
    {
        public static void ConfigureSwaggerDocument(this IServiceCollection services, string version, string application)
        {
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = version;
                    document.Info.Title = $"VXDESIGN.STORE: Development Tools — {application} API";
                    document.Info.Contact = new OpenApiContact
                    {
                        Name = "Roman Mashenkin",
                        Email = string.Empty,
                        Url = "https://github.com/GUSAR1T0"
                    };
                    document.Info.License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = "https://opensource.org/licenses/MIT"
                    };
                };
            });
        }
    }
}