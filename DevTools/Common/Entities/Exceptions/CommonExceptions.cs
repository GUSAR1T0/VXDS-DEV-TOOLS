using VXDesign.Store.DevTools.Common.Models.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public static class CommonExceptions
    {
        #region Camunda / SRS

        public static BadRequestException CamundaEndpointIsNotFoundByActionCode() => new BadRequestException("Failed to find a endpoint by action code");

        public static BadRequestException SyrinxHasSentErrorResponse<TModel>(TModel model) where TModel : ICamundaResponseModel =>
            new BadRequestException($"Failed to send request to Camunda through Syrinx: {model.Status} \"{model.Reason}\"");

        #endregion

        #region Authentication

        public static BadRequestException InvalidEmailOrPassword() => new BadRequestException("Failed to authenticate user due to invalid email or password");

        public static BadRequestException NoAuthenticationData() => new BadRequestException("No data for authentication");

        public static BadRequestException RefreshTokensAreDifferent() => new BadRequestException("Refresh tokens are different");
        
        public static BadRequestException InvalidTokenInHeader() => new BadRequestException("Failed to define a token from header");
        
        public static BadRequestException UserHasAlreadyExist() => new BadRequestException("User with this email has already exist");

        #endregion
    }
}