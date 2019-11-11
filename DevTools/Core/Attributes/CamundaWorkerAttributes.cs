using System;
using VXDesign.Store.DevTools.Core.Enums.Camunda;

namespace VXDesign.Store.DevTools.Core.Attributes
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