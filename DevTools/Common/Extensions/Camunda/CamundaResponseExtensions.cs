using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VXDesign.Store.DevTools.Common.Containers.Camunda.Base;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Extensions.Base;
using VXDesign.Store.DevTools.Common.Extensions.HTTP;
using VXDesign.Store.DevTools.Common.Utils.Camunda;

namespace VXDesign.Store.DevTools.Common.Extensions.Camunda
{
    internal static class CamundaResponseExtensions
    {
        internal static TResponse PostHandle<TResponse>(this TResponse response) where TResponse : ICamundaResponse
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new ResponseJsonResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            if (!response.IsWithoutErrors()) throw CommonExceptions.SyrinxHasSentErrorResponse(response);

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