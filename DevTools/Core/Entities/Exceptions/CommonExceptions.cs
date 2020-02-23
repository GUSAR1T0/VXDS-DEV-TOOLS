using Microsoft.AspNetCore.Http;
using VXDesign.Store.DevTools.Core.Entities.Camunda.Base;
using VXDesign.Store.DevTools.Core.Entities.Operations;

namespace VXDesign.Store.DevTools.Core.Entities.Exceptions
{
    public static class CommonExceptions
    {
        #region System

        public static OperationException TransactionHasAlreadyBegun(IOperation operation) => new OperationException(operation, "Transaction has already begun");

        public static OperationException OperationHasAlreadyCompleted(IOperation operation) => new OperationException(operation, "Operation has already completed");

        #endregion

        #region Camunda / SRS

        public static NotFoundException CamundaEndpointIsNotFoundByActionCode(IOperation operation) => new NotFoundException(operation, "Failed to find a Camunda endpoint by action code");

        public static BadRequestException SyrinxHasSentErrorResponse<TModel>(IOperation operation, TModel model) where TModel : ICamundaResponse
        {
            return new BadRequestException(operation, $"Failed to send request to Camunda through Syrinx: {model.Status} \"{model.Reason}\"");
        }

        public static CamundaWorkersBuilderException PropertiesAreEmpty() => new CamundaWorkersBuilderException("Couldn't launch workers because properties are empty");

        public static CamundaWorkersBuilderException LogScopeIsNotStated() => new CamundaWorkersBuilderException("Couldn't launch workers because log scope is not stated");

        #endregion

        #region GitHub

        public static NotFoundException GitHubTokenIsNotStated(IOperation operation) => new NotFoundException(operation, "Failed to connect to GitHub due to missing token in the system");

        public static NotFoundException GitHubEndpointIsNotFoundByEndpointCode(IOperation operation) => new NotFoundException(operation, "Failed to find a GitHub endpoint by endpoint code");

        #endregion

        #region Authentication / Users

        public static BadRequestException RegistrationIsFailed(IOperation operation) => new BadRequestException(operation, "Failed to register user");

        public static BadRequestException UserHasAlreadyAuthenticated(IOperation operation) => new BadRequestException(operation, "User has already authenticated");

        public static BadRequestException NoAuthenticationData(IOperation operation) => new BadRequestException(operation, "No data for authentication");

        public static BadRequestException RefreshTokensAreDifferent(IOperation operation) => new BadRequestException(operation, "Refresh tokens are different");

        public static BadRequestException InvalidTokenInHeader(IOperation operation) => new BadRequestException(operation, "Failed to define a token from header");

        public static BadRequestException UserHasAlreadyExist(IOperation operation) => new BadRequestException(operation, "User with this email has already exist");

        public static BadRequestException FailedToReadAuthenticationDataFromClaims(IOperation operation) => new BadRequestException(operation, "Failed to read authentication data from claims");

        public static BadRequestException AuthenticationFailed(IOperation operation) => new BadRequestException(operation, "Authentication failed");

        public static NotFoundException UserWasNotFound(IOperation operation, int id) => new NotFoundException(operation, $"User with ID \"{id}\" was not found");

        public static NotFoundException UserRoleWasNotFound(IOperation operation, int id) => new NotFoundException(operation, $"User role with ID \"{id}\" was not found");

        public static NotFoundException UserRoleHasAlreadyExisted(IOperation operation, string name) => new NotFoundException(operation, $"User role with name \"{name}\" has already existed");

        public static AuthenticationException AccessDenied(IOperation operation, int statusCode, bool deactivated = false)
        {
            var message = "Access denied";

            switch (statusCode)
            {
                case StatusCodes.Status401Unauthorized when !deactivated:
                    message += " (the request is unauthorized)";
                    break;
                case StatusCodes.Status401Unauthorized:
                    message += " (user is deactivated)";
                    break;
                case StatusCodes.Status403Forbidden:
                    message += " (user doesn't have permissions for operation)";
                    break;
                default:
                    break;
            }

            return new AuthenticationException(operation, message, statusCode);
        }

        public static BadRequestException CouldNotChangeOwnAccountSpecificInfo(IOperation operation) => new BadRequestException(operation, "Failed to change account specific info for yourself");

        #endregion
    }
}