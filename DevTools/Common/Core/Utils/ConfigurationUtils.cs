using Microsoft.Extensions.Configuration;
using NLog.Config;
using NLog.Targets;

namespace VXDesign.Store.DevTools.Common.Core.Utils
{
    public static class ConfigurationUtils
    {
        public static LoggingConfiguration GetLoggerConfiguration()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ConsoleTarget("ConsoleOutput")
            {
                Layout = @" ${uppercase:${level}:padding=5} | ${longdate} | ${logger}${newline}       | ${message} ${exception}"
            };

            config.AddTarget(consoleTarget);
            config.AddRuleForAllLevels(consoleTarget);
            return config;
        }

        public static IConfiguration GetEnvironmentConfiguration(string environment = null)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true);

            if (!string.IsNullOrWhiteSpace(environment))
            {
                builder.AddJsonFile($"appsettings.{environment}.json", true, true);
            }

            builder.AddEnvironmentVariables();
            return builder.Build();
        }
    }
}