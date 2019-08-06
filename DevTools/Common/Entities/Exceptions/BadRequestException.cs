using System;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}