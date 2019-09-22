using System;
using System.Collections;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace VXDesign.Store.DevTools.Common.Utils.Base
{
    public static class ConfigurationUtils
    {
        public static IConfiguration GetConfiguration()
        {
            var environmentVariables = Environment.GetEnvironmentVariables().Cast<DictionaryEntry>()
                .ToDictionary(environmentVariable => environmentVariable.Key.ToString(), environmentVariable => environmentVariable.Value?.ToString());
            return new ConfigurationBuilder().AddInMemoryCollection(environmentVariables).Build();
        }
    }
}