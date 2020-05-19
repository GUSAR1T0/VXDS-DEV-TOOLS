﻿using System;
using VXDesign.Store.DevTools.Common.Core.Utils;
using VXDesign.Store.DevTools.Common.Migrations.Camunda;
using VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workflows.Properties;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workflows
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = ConfigurationUtils.GetEnvironmentConfiguration(environment);
            var properties = PropertiesUtils.Create<ProjectProperties>(configuration);
            ConsoleApplicationUtils.Launch(() => CamundaDeploymentUtils.Perform(args, properties.DatabaseConnectionProperties, properties.SyrinxProperties, typeof(Program).Assembly));
        }
    }
}