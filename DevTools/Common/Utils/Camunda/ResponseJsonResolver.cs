using System;
using Newtonsoft.Json.Serialization;
using VXDesign.Store.DevTools.Common.Entities.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Utils.Camunda
{
    internal class ResponseJsonResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type type)
        {
            var contract = base.CreateContract(type);

            if (type == typeof(ICamundaVariable))
            {
                contract.Converter = new VariableJsonConverter();
            }

            return contract;
        }
    }
}