using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Core.Extensions
{
    public static class ResponseExtensions
    {
        public static bool IsWithoutErrors(this IResponse response) => response.Status < 400;
    }
}