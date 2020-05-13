using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Utils;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Extensions
{
    internal static class CamundaResponseExtensions
    {
        internal static TResponse PostHandle<TResponse>(this TResponse response, IOperation operation) where TResponse : ICamundaResponse
        {
            if (!response.IsWithoutErrors()) throw CommonExceptions.CamundaRequestCanNotBeSent(operation, response.Status, response.Reason, response.Output);

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamundaResponseJsonResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            var intermediateResponse = JsonConvert.DeserializeObject<IntermediateCamundaResponse<object>>(response.Output, settings);
            if (intermediateResponse.IsWithoutErrors())
            {
                var property = response.GetType().GetProperty(nameof(IntermediateCamundaResponse<object>.Response));
                var value = JsonConvert.DeserializeObject(intermediateResponse.Output, property.PropertyType, settings);
                property.SetPropertyValue(response, value);
            }
            else
            {
                response.Errors = JsonConvert.DeserializeObject(intermediateResponse.Output, settings);
            }

            response.Status = intermediateResponse.Status;
            response.Output = intermediateResponse.Output;
            response.Reason = intermediateResponse.Reason;

            return response;
        }
    }
}