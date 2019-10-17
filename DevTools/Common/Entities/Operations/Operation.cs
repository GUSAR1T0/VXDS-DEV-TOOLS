using System;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Entities.Operations
{
    public interface IOperation
    {
    }

    public class Operation : IOperation, IDisposable
    {
        private readonly DatabaseConnectionProperties properties;

        public Operation(DatabaseConnectionProperties properties)
        {
            this.properties = properties;
        }

        public void Dispose()
        {
        }
    }
}