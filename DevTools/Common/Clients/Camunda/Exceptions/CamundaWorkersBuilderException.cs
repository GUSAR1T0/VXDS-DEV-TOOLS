using System;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Exceptions
{
    public class CamundaWorkersBuilderException : Exception
    {
        public CamundaWorkersBuilderException(string message) : base(message)
        {
        }

        public static CamundaWorkersBuilderException PropertiesAreEmpty() => new CamundaWorkersBuilderException("Couldn't launch workers because properties are empty");

        public static CamundaWorkersBuilderException LogScopeIsNotStated() => new CamundaWorkersBuilderException("Couldn't launch workers because log scope is not stated");
    }
}