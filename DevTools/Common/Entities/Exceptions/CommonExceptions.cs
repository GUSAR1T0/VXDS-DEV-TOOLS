using VXDesign.Store.DevTools.Common.Models.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public static class CommonExceptions
    {
        public static BadRequestException CamundaEndpointIsNotFoundByActionCode() => new BadRequestException("Failed to find a endpoint by action code");

        public static BadRequestException SyrinxHasSentErrorResponse<TModel>(TModel model) where TModel : ICamundaResponseModel =>
            new BadRequestException($"Failed to send request to Camunda through Syrinx: {model.Status} \"{model.Reason}\"");
    }
}