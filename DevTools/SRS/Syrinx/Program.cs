﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace VXDesign.Store.DevTools.SRS.Syrinx
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}