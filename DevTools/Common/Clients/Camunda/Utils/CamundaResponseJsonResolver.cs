using System;
using Newtonsoft.Json.Serialization;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Utils
{
    internal class CamundaResponseJsonResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type type)
        {
            var contract = base.CreateContract(type);

            if (type == typeof(ICamundaVariable))
            {
                contract.Converter = new CamundaVariableJsonConverter();
            }

            return contract;
        }
    }
}