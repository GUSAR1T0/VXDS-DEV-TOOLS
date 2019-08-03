using System;

namespace VXDesign.Store.DevTools.SRS.Camunda.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class CamundaCategoryAttribute : Attribute
    {
        internal string Name { get; }
        internal string Root { get; }

        internal CamundaCategoryAttribute(string name, string root)
        {
            Name = name;
            Root = root;
        }
    }
}