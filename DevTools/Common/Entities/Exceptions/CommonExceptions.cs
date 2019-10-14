using VXDesign.Store.DevTools.Common.Containers.Camunda.Base;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public static class CommonExceptions
    {
        #region Camunda / SRS

        public static BadRequestException CamundaEndpointIsNotFoundByActionCode() => new BadRequestException("Failed to find a endpoint by action code");

        public static BadRequestException SyrinxHasSentErrorResponse<TModel>(TModel model) where TModel : ICamundaResponse =>
            new BadRequestException($"Failed to send request to Camunda through Syrinx: {model.Status} \"{model.Reason}\"");

        public static CamundaWorkersBuilderException PropertiesAreEmpty() => new CamundaWorkersBuilderException("Couldn't launch workers because properties are empty");

        #endregion

        #region Authentication

        public static BadRequestException InvalidEmailOrPassword() => new BadRequestException("Failed to authenticate user due to invalid email or password");

        public static BadRequestException RegistrationIsFailed(string message) => new BadRequestException($"Failed to register user due to the following reasons: {message}");

        public static BadRequestException NoAuthenticationData() => new BadRequestException("No data for authentication");

        public static BadRequestException RefreshTokensAreDifferent() => new BadRequestException("Refresh tokens are different");

        public static BadRequestException InvalidTokenInHeader() => new BadRequestException("Failed to define a token from header");

        public static BadRequestException UserHasAlreadyExist() => new BadRequestException("User with this email has already exist");

        #endregion

        #region Users

        public static BadRequestException FailedToGetProfileDueToMissedEmail() => new BadRequestException("Failed to get user profile due to missed email");

        public static BadRequestException FailedToUpdateProfileDueToMissedId() => new BadRequestException("Failed to update user profile due to missed ID");

        public static NotFoundException UserWasNotFound(string email = null)
        {
            var message = !string.IsNullOrWhiteSpace(email) ? $"User with email \"{email}\" was not found" : "Requested user was not found";
            return new NotFoundException(message);
        }

        #endregion
    }
}