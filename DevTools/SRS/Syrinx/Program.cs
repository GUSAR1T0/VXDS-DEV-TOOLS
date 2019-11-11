using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using VXDesign.Store.DevTools.Core.Extensions.Controllers;

namespace VXDesign.Store.DevTools.SRS.Syrinx
{
    public class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().LaunchWebApplication();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseNLogExtension();
    }
}