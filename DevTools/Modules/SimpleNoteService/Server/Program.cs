using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using VXDesign.Store.DevTools.Common.Core.Extensions;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server
{
    public class Program
    {
        public static void Main(string[] args) => CreateWebHostBuilder(args).Build().LaunchWebApplication();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseNLogExtension();
    }
}