﻿using System;
using VXDesign.Store.DevTools.Common.Core.Utils;
using VXDesign.Store.DevTools.Common.Migrations.Database;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Database.Properties;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Database
{
    class Program
    {
        static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = ConfigurationUtils.GetEnvironmentConfiguration(environment);
            var properties = PropertiesUtils.Create<ProjectProperties>(configuration);
            ConsoleApplicationUtils.Launch(() => DatabaseMigrationUtils.Perform(args, properties.DatabaseConnectionProperties, typeof(Program).Assembly));
        }
    }
}