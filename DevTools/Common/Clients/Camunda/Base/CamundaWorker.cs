using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Base
{
    public abstract class CamundaWorker
    {
        private class VariableProperty
        {
            internal PropertyInfo Property { get; set; }
            internal CamundaWorkerVariableAttribute Attribute { get; set; }
            internal string GetVariableName => Attribute.Name ?? Property.Name;
        }

        private static IEnumerable<VariableProperty> GetVariablePropertiesAndAttributes(Type type, CamundaVariableDirection direction) => type
            .GetProperties()
            .Select(property => new VariableProperty
            {
                Property = property,
                Attribute = property.GetCustomAttributes<CamundaWorkerVariableAttribute>(false).FirstOrDefault()
            })
            .Where(arg => arg.Attribute != null && arg.Attribute.Direction.HasFlag(direction))
            .ToList();

        internal static IEnumerable<string> InputVariableNames(Type type) => GetVariablePropertiesAndAttributes(type, CamundaVariableDirection.Input).Select(variable => variable.GetVariableName);

        internal void InitializeVariables(IReadOnlyCamundaVariables variables)
        {
            var properties = GetVariablePropertiesAndAttributes(GetType(), CamundaVariableDirection.Input);
            foreach (var property in properties)
            {
                if (variables.ContainsKey(property.GetVariableName))
                {
                    var variable = variables[property.GetVariableName];
                    var type = GetTypeOfVariable(property.Property.PropertyType, variable);
                    property.Property.SetPropertyValue(this, variable?.To(type));
                }
            }
        }

        private static Type GetTypeOfVariable(Type propertyType, ICamundaVariable variable)
        {
            if (!propertyType.IsEnum)
            {
                var underlyingType = Nullable.GetUnderlyingType(propertyType);
                if (underlyingType == null || !underlyingType.IsEnum)
                {
                    return propertyType;
                }
            }

            return variable switch
            {
                ShortVariable _ => typeof(short?),
                IntegerVariable _ => typeof(int?),
                LongVariable _ => typeof(long?),
                _ => propertyType
            };
        }

        internal ICamundaVariables CollectVariables()
        {
            var properties = GetVariablePropertiesAndAttributes(GetType(), CamundaVariableDirection.Output);
            var variables = new CamundaVariables();
            foreach (var property in properties)
            {
                var variableName = property.GetVariableName;
                var propertyInfo = property.Property;
                var variableValue = propertyInfo.GetValue(this);

                if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
                {
                    variables.Add(variableName, (bool?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(byte[]))
                {
                    variables.Add(variableName, (byte[]) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(short) || propertyInfo.PropertyType == typeof(short?) ||
                         propertyInfo.PropertyType.GetUnderlyingTypeFromPossibleEnumType() == typeof(short))
                {
                    if (variableValue == null)
                    {
                        variables.Add(variableName, (short?) null);
                    }
                    else
                    {
                        variables.Add(variableName, (short) variableValue);
                    }
                }
                else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?) ||
                         propertyInfo.PropertyType.GetUnderlyingTypeFromPossibleEnumType() == typeof(int))
                {
                    if (variableValue == null)
                    {
                        variables.Add(variableName, (int?) null);
                    }
                    else
                    {
                        variables.Add(variableName, (int) variableValue);
                    }
                }
                else if (propertyInfo.PropertyType == typeof(long) || propertyInfo.PropertyType == typeof(long?) ||
                         propertyInfo.PropertyType.GetUnderlyingTypeFromPossibleEnumType() == typeof(long))
                {
                    if (variableValue == null)
                    {
                        variables.Add(variableName, (long?) null);
                    }
                    else
                    {
                        variables.Add(variableName, (long) variableValue);
                    }
                }
                else if (propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(double?))
                {
                    variables.Add(variableName, (double?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(decimal) || propertyInfo.PropertyType == typeof(decimal?))
                {
                    variables.Add(variableName, (decimal?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                {
                    variables.Add(variableName, (DateTime?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(DateTimeOffset) || propertyInfo.PropertyType == typeof(DateTimeOffset?))
                {
                    variables.Add(variableName, (DateTimeOffset?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    variables.Add(variableName, (string) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(CamundaFile))
                {
                    variables.Add(variableName, (CamundaFile) variableValue);
                }
                else
                {
                    variables.Add(property.GetVariableName, propertyInfo.GetValue(this));
                }
            }

            return variables;
        }

        public abstract void Execute(IOperation operation, IOperationLogger logger);
    }
}