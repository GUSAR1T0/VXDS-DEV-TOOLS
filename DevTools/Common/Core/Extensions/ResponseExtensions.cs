using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Core.Extensions
{
    public static class ResponseExtensions
    {
        public static bool IsWithoutErrors(this IResponse response) => response.Status < 400;
        
        public static dynamic ToLog(this IResponse response) => new
        {
            response.Status,
            response.Reason,
            response.Output
        };
    }
}