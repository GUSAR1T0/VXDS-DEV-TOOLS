using System.Collections.Generic;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Containers.Camunda.Base;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Extensions.Base;
using VXDesign.Store.DevTools.Common.Extensions.HTTP;
using VXDesign.Store.DevTools.Common.Utils.Camunda;

namespace VXDesign.Store.DevTools.Common.Extensions.Camunda
{
    internal static class CamundaResponseModelExtensions
    {
        internal static TModel PostHandle<TModel>(this TModel model) where TModel : ICamundaResponseModel
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new ResponseJsonResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            if (!model.IsWithoutErrors()) throw CommonExceptions.SyrinxHasSentErrorResponse(model);

            var response = JsonConvert.DeserializeObject<IntermediateCamundaResponseModel<object>>(model.Output, settings);

            if (response.IsWithoutErrors())
            {
                var property = model.GetType().GetProperty(nameof(IntermediateCamundaResponseModel<object>.Model));
                var value = JsonConvert.DeserializeObject(response.Output, property.PropertyType, settings);
                property.SetPropertyValue(model, value);
            }
            else
            {
                model.Errors = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Output, settings);
            }

            model.Status = response.Status;
            model.Output = response.Output;
            model.Reason = response.Reason;

            return model;
        }
    }
}