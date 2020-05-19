using System;
using Newtonsoft.Json;

namespace VXDesign.Store.DevTools.Common.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static bool IsEmpty(this Exception exception) => exception == null;

        public static dynamic ToLog(this Exception exception) => new
        {
            Type = exception.GetType().FullName,
            exception.Source,
            Data = JsonConvert.SerializeObject(exception.Data),
            exception.Message,
            exception.StackTrace
        };
    }
}