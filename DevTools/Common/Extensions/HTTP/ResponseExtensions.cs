using VXDesign.Store.DevTools.Common.Entities.HTTP;

namespace VXDesign.Store.DevTools.Common.Extensions.HTTP
{
    public static class ResponseExtensions
    {
        public static bool IsWithoutErrors(this IResponse response) => response.Status < 400;
    }
}