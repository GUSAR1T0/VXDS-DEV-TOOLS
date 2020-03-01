using System;
using NLog;

namespace VXDesign.Store.DevTools.Common.Core.Utils
{
    public static class ConsoleApplicationUtils
    {
        public static void Launch(Action main)
        {
            try
            {
                LogManager.Configuration = ConfigurationUtils.GetLoggerConfiguration();
                main();
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}