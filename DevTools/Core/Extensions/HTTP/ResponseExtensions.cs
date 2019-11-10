using VXDesign.Store.DevTools.Core.Entities.HTTP;

namespace VXDesign.Store.DevTools.Core.Extensions.HTTP
{
    public static class ResponseExtensions
    {
        public static bool IsWithoutErrors(this IResponse response) => response.Status < 400;
    }
}