using System.IO;
using Microsoft.Extensions.Configuration;
using NLog.Config;
using NLog.Targets;

namespace VXDesign.Store.DevTools.Common.Utils.Base
{
    public static class ConfigurationUtils
    {
        public static LoggingConfiguration GetLoggerConfiguration()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget("ConsoleOutput")
            {
                Layout = @"${longdate} | ${uppercase:${level}} | ${logger} | ${message} ${exception}"
            };
            config.AddTarget(consoleTarget);
            config.AddRuleForAllLevels(consoleTarget);
            return config;
        }

        public static IConfiguration GetEnvironmentConfiguration() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .Build();
    }
}