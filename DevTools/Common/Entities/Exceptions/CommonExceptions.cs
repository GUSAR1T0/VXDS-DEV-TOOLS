using VXDesign.Store.DevTools.Common.Entities.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public static class CommonExceptions
    {
        #region Camunda / SRS

        public static NotFoundException CamundaEndpointIsNotFoundByActionCode() => new NotFoundException("Failed to find a endpoint by action code");

        public static BadRequestException SyrinxHasSentErrorResponse<TModel>(TModel model) where TModel : ICamundaResponse =>
            new BadRequestException($"Failed to send request to Camunda through Syrinx: {model.Status} \"{model.Reason}\"");

        public static CamundaWorkersBuilderException PropertiesAreEmpty() => new CamundaWorkersBuilderException("Couldn't launch workers because properties are empty");

        #endregion

        #region Authentication / Users

        public static BadRequestException RegistrationIsFailed() => new BadRequestException("Failed to register user");

        public static BadRequestException NoAuthenticationData() => new BadRequestException("No data for authentication");

        public static BadRequestException RefreshTokensAreDifferent() => new BadRequestException("Refresh tokens are different");

        public static BadRequestException InvalidTokenInHeader() => new BadRequestException("Failed to define a token from header");

        public static BadRequestException UserHasAlreadyExist() => new BadRequestException("User with this email has already exist");

        public static BadRequestException FailedToReadAuthenticationDataFromClaims() => new BadRequestException("Failed to read authentication data from claims");

        public static NotFoundException UserWasNotFound(string email = null)
        {
            var message = !string.IsNullOrWhiteSpace(email) ? $"User with email \"{email}\" was not found" : "Requested user was not found";
            return new NotFoundException(message);
        }

        public static BadRequestException FailedToGetProfileDueToMissedEmail() => new BadRequestException("Failed to get user profile due to missed email");

        #endregion
    }
}