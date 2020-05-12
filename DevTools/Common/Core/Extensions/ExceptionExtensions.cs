using System;

namespace VXDesign.Store.DevTools.Common.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static bool IsEmpty(this Exception exception) => exception == null;
    }
}