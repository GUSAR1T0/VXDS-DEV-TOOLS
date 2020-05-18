using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Core.Exceptions
{
    public static class CommonExceptions
    {
        #region System

        public static ArgumentException DatabaseCouldNotBeMigrated() => new ArgumentException("Database couldn't be migrated due to failed program arguments");

        public static ArgumentException CamundaCouldNotBeDeployed() => new ArgumentException("Camunda couldn't be deployed due to failed program arguments");

        public static OperationException TransactionHasAlreadyBegun(IOperation operation) => new OperationException(operation, "Transaction has already begun");

        public static OperationException OperationHasAlreadyCompleted(IOperation operation) => new OperationException(operation, "Operation has already completed");

        public static NotFoundException OperationWasNotFound(IOperation operation, long operationId) => new NotFoundException(operation, $"Operation with ID \"{operationId}\" wasn't found");

        #endregion

        #region Database

        public static NotFoundException DatabaseSchemaWasNotFound(IOperation operation, string schema) => new NotFoundException(operation, $"Database schema \"{schema}\" wasn't found");

        #endregion

        #region Camunda / SRS

        public static NotFoundException CamundaEndpointIsNotFoundByActionCode(IOperation operation) => new NotFoundException(operation, "Failed to find a Camunda endpoint by action code");

        public static BadRequestException FailedToGetCamundaVersion(IOperation operation) => new BadRequestException(operation, "Failed to get a Camunda version");

        public static BadRequestException CamundaRequestCanNotBeSent(IOperation operation, int status, string reason, string message)
        {
            return new BadRequestException(operation, $"Failed to send request to Camunda through Syrinx: {status} \"{reason}\":\n{message}");
        }

        public static BadRequestException CamundaRequestIsFailed(IOperation operation, int status, string reason, string message)
        {
            return new BadRequestException(operation, $"Failed to send request to Camunda: {status} \"{reason}\":\n{message}");
        }

        public static BadRequestException FailedToDeployCamundaDueToMissedSettings(IOperation operation)
        {
            return new BadRequestException(operation, "Failed to deploy Camunda workflows due to missed settings");
        }

        public static BadRequestException FailedToDeployCamundaDueToUndefinedFileList(IOperation operation)
        {
            return new BadRequestException(operation, "Failed to deploy Camunda workflows due to undefined file list");
        }

        public static BadRequestException FailedToDeployCamundaWorkflows(IOperation operation)
        {
            return new BadRequestException(operation, "Failed to deploy Camunda workflows");
        }

        public static BadRequestException FailedToGetListOfCamundaDeployments(IOperation operation)
        {
            return new BadRequestException(operation, "Failed to get list of Camunda deployments");
        }

        public static BadRequestException FailedToDeleteCamundaDeployment(IOperation operation)
        {
            return new BadRequestException(operation, "Failed to delete Camunda deployment");
        }

        #endregion

        #region Hosts

        public static NotFoundException HostWasNotFound(IOperation operation, int id) => new NotFoundException(operation, $"Host with ID \"{id}\" was not found");

        public static BadRequestException HostNameIsNotUnique(IOperation operation) => new BadRequestException(operation, "Host name is not unique");

        public static BadRequestException FailedToSendCommandToRemoteHost(IOperation operation, string command, int exitStatus, string error)
        {
            return new BadRequestException(operation, $"Failed to send command \"{command}\" to remote host, exit status \"{exitStatus}\":\n{error}");
        }

        #endregion

        #region Projects

        #region GitHub

        public static NotFoundException GitHubTokenIsNotStated(IOperation operation) => new NotFoundException(operation, "Failed to connect to GitHub due to missing token in the system");

        public static NotFoundException GitHubEndpointIsNotFoundByEndpointCode(IOperation operation) => new NotFoundException(operation, "Failed to find a GitHub endpoint by endpoint code");

        public static BadRequestException UserRepositoriesCouldNotBeLoaded(IOperation operation, string message) =>
            new BadRequestException(operation, $"Failed to retrieve all user repositories: {message}");

        public static BadRequestException RepositoryLanguagesCouldNotBeLoaded(IOperation operation, string message) =>
            new BadRequestException(operation, $"Failed to retrieve all repository languages: {message}");

        #endregion

        public static BadRequestException ProjectHasAlreadyExisted(IOperation operation, IEnumerable<byte> errorCodes, string name, string alias, long? gitHubRepoId)
        {
            var errors = new List<string>();
            foreach (var errorCode in errorCodes)
            {
                switch (errorCode)
                {
                    case 1:
                        errors.Add($"name \"{name}\"");
                        break;
                    case 2:
                        errors.Add($"alias \"{alias}\"");
                        break;
                    case 3:
                        errors.Add($"GitHub Repository ID \"{gitHubRepoId}\"");
                        break;
                }
            }

            return new BadRequestException(operation, $"Project with {string.Join(", ", errors)} has already existed");
        }

        public static NotFoundException ProjectWasNotFound(IOperation operation, int id) => new NotFoundException(operation, $"Project with ID \"{id}\" was not found");

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

        public static BadRequestException UserRoleHasAlreadyExisted(IOperation operation, string name) => new BadRequestException(operation, $"User role with name \"{name}\" has already existed");

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

        public static NotFoundException PermissionGroupWasNotFound(IOperation operation, string name) => new NotFoundException(operation, $"Permission group \"{name}\" was not found into database");

        #endregion

        #region Incidents

        public static BadRequestException IncidentAlreadyExists(IOperation operation, long operationId)
        {
            return new BadRequestException(operation, $"Incident for operation with ID \"{operationId}\" already exists");
        }

        public static NotFoundException IncidentWasNotFound(IOperation operation, long operationId)
        {
            return new NotFoundException(operation, $"Incident for operation with ID \"{operationId}\" wasn't found");
        }

        public static NotFoundException IncidentHistoryChangeWasNotFound(IOperation operation, long operationId, long historyId)
        {
            return new NotFoundException(operation, $"Incident history change with ID \"{historyId}\" for operation with ID \"{operationId}\" wasn't found");
        }

        #endregion

        #region Notifications

        public static NotFoundException NotificationWasNotFound(IOperation operation, int notificationId)
        {
            return new NotFoundException(operation, $"Notification with ID \"{notificationId}\" wasn't found");
        }

        #endregion

        #region Folder / Note

        public static NotFoundException FolderWasNotFound(IOperation operation, int id) => new NotFoundException(operation, $"Folder with ID \"{id}\" was not found");

        public static NotFoundException NoteWasNotFound(IOperation operation, int id) => new NotFoundException(operation, $"Note with ID \"{id}\" was not found");

        public static BadRequestException NoteTitleIsEmpty(IOperation operation) => new BadRequestException(operation, "Note title can't be empty");

        #endregion

        #region Files

        public static BadRequestException UnexpectedExtensionOfUploadedFile(IOperation operation, string fileExtension, params FileExtension[] expectedExtensions)
        {
            var message = $"Unexpected uploaded file extension \"{fileExtension}\" is, use [{string.Join(", ", expectedExtensions.Select(x => x.GetDescription()))}] instead";
            return new BadRequestException(operation, message);
        }

        public static BadRequestException UploadedFileExtensionIsUndefined(IOperation operation, params FileExtension[] expectedExtensions)
        {
            return new BadRequestException(operation, $"Uploaded file extension is undefined, use [{string.Join(", ", expectedExtensions.Select(x => x.GetDescription()))}]");
        }

        public static NotFoundException FileWasNotFound(IOperation operation, int fileId) => new NotFoundException(operation, $"File with ID \"{fileId}\" was not found");

        #endregion

        #region Modules

        public static BadRequestException ModuleConfigurationIsInvalid(IOperation operation) => new BadRequestException(operation, "Uploaded module configuration is invalid");

        public static BadRequestException ModuleConfigurationsHaveDifferentLength(IOperation operation)
        {
            return new BadRequestException(operation, "Uploaded and stored module configurations have different length");
        }

        public static NotFoundException ModuleConfigurationWasNotFound(IOperation operation)
        {
            return new NotFoundException(operation, "Module configuration wasn't found");
        }

        public static NotFoundException FailedToDefineModuleForSubmission(IOperation operation)
        {
            return new NotFoundException(operation, "Failed to define module to submit configuration by file");
        }

        public static NotFoundException FailedToDefineModuleForUpgrade(IOperation operation)
        {
            return new NotFoundException(operation, "Failed to define module to upgrade configuration by file");
        }

        public static BadRequestException FailedToSubmitModuleConfiguration(IOperation operation, ModuleConfigurationVerdict verdict)
        {
            var message = $"Failed to submit module configuration due to conflicts between module and new configuration versions, module verdict: {verdict.GetDescription()}";
            return new BadRequestException(operation, message);
        }

        public static NotFoundException ModuleWasNotFound(IOperation operation, int moduleId) => new NotFoundException(operation, $"Module with ID \"{moduleId}\" was not found");

        public static BadRequestException FailedToRunModule(IOperation operation) => new BadRequestException(operation, "Failed to run module because it's not ready for that");

        public static BadRequestException FailedToStopModule(IOperation operation) => new BadRequestException(operation, "Failed to stop module because it's not ready for that");

        public static BadRequestException FailedToUninstallModule(IOperation operation) => new BadRequestException(operation, "Failed to uninstall module because it's not ready for that");

        public static BadRequestException FailedToReinstallModule(IOperation operation) => new BadRequestException(operation, "Failed to reinstall module because it's not ready for that");

        public static BadRequestException FailedToUpgradeModuleConfigurationDueToModuleStatus(IOperation operation)
        {
            return new BadRequestException(operation, "Failed to upgrade module configuration because module is not ready for that");
        }

        public static BadRequestException FailedToDowngradeModuleConfigurationDueToModuleStatus(IOperation operation)
        {
            return new BadRequestException(operation, "Failed to downgrade module configuration because module is not ready for that");
        }

        public static NotFoundException NoModuleConfigurationForUpgrade(IOperation operation) => new NotFoundException(operation, "No module configuration is found for upgrade of module");

        public static BadRequestException NoModuleConfigurationForDowngrade(IOperation operation) => new BadRequestException(operation, "No module configuration is found for downgrade of module");

        public static BadRequestException FailedToUpgradeModuleConfigurationDueToVerdict(IOperation operation)
        {
            return new BadRequestException(operation, "Failed to upgrade module configuration because it can't be confirmed by module verdict");
        }

        public static BadRequestException FailedToDowngradeModuleConfigurationDueToVerdict(IOperation operation)
        {
            return new BadRequestException(operation, "Failed to downgrade module configuration because it can't be confirmed by module verdict");
        }

        public static BadRequestException FailedToUpgradeModuleConfigurationDueToOperatingSystemConflict(IOperation operation)
        {
            return new BadRequestException(operation, "Failed to upgrade module configuration because it can't be confirmed by host operating system");
        }

        public static BadRequestException FailedToDowngradeModuleConfigurationDueToOperatingSystemConflict(IOperation operation)
        {
            return new BadRequestException(operation, "Failed to downgrade module configuration because it can't be confirmed by host operating system");
        }

        #endregion
    }
}