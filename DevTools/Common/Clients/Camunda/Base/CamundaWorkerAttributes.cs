using System;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Base
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CamundaWorkerTopicAttribute : Attribute
    {
        public string Name { get; }

        public CamundaWorkerTopicAttribute(string name)
        {
            Name = name;
        }
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class CamundaWorkerVariableAttribute : Attribute
    {
        public string Name { get; set; }
        public CamundaVariableDirection Direction { get; set; } = CamundaVariableDirection.Input | CamundaVariableDirection.Output;
    }
}