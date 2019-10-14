using System;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}