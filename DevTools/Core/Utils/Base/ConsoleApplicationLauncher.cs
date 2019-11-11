using System;
using NLog;

namespace VXDesign.Store.DevTools.Core.Utils.Base
{
    public static class ConsoleApplicationLauncher
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