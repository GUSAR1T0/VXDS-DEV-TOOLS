using Newtonsoft.Json;
using VXDesign.Store.DevTools.Core.Entities.Camunda.Base;
using VXDesign.Store.DevTools.Core.Entities.Exceptions;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Extensions.Base;
using VXDesign.Store.DevTools.Core.Extensions.HTTP;
using VXDesign.Store.DevTools.Core.Utils.Camunda;

namespace VXDesign.Store.DevTools.Core.Extensions.Camunda
{
    internal static class CamundaResponseExtensions
    {
        internal static TResponse PostHandle<TResponse>(this TResponse response, IOperation operation) where TResponse : ICamundaResponse
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new ResponseJsonResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            if (!response.IsWithoutErrors()) throw CommonExceptions.SyrinxHasSentErrorResponse(operation, response);

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