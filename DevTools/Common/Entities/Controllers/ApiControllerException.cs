using System;

namespace VXDesign.Store.DevTools.Common.Entities.Controllers
{
    public class ApiControllerException : Exception
    {
        public ApiControllerException(string message) : base(message)
        {
        }
    }
}