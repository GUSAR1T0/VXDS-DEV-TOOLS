using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Clients.Camunda;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Entities;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Models.Deployment;
using VXDesign.Store.DevTools.Common.Core.Entities.File;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Migrations.Common;

namespace VXDesign.Store.DevTools.Common.Migrations.Camunda
{
    public interface ICamundaDeploymentService : IMigrationService
    {
    }

    public class CamundaDeploymentService : ICamundaDeploymentService
    {
        private readonly CamundaDeploymentParameters camundaDeploymentParameters;
        private readonly IOperationService operationService;
        private readonly ISyrinxCamundaClientService syrinxCamundaClientService;

        private CamundaDeploymentSettings GetSettings(IOperation operation) =>
        (
            from type in camundaDeploymentParameters.Assembly.GetExportedTypes()
            where typeof(CamundaDeploymentSettings).IsAssignableFrom(type)
            select type.GetConstructor(Type.EmptyTypes)?.Invoke(new object[0]) as CamundaDeploymentSettings
        ).FirstOrDefault() ?? throw CommonExceptions.FailedToDeployCamundaDueToMissedSettings(operation);

        private List<LocalFile> GetFiles(IOperation operation)
        {
            var location = Path.Combine(Path.GetDirectoryName(camundaDeploymentParameters.Assembly.Location), "Workflows");
            var files = Directory.GetFiles(location, "*.bpmn", SearchOption.AllDirectories);

            if (files?.Any() != true)
            {
                throw CommonExceptions.FailedToDeployCamundaDueToUndefinedFileList(operation);
            }

            return files.Select(file =>
            {
                var fileName = Path.GetFileName(file);
                var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                return new LocalFile
                {
                    Name = "files",
                    FileName = fileName,
                    Stream = stream
                };
            }).ToList();
        }

        public CamundaDeploymentService(CamundaDeploymentParameters camundaDeploymentParameters, IOperationService operationService, ISyrinxCamundaClientService syrinxCamundaClientService)
        {
            this.camundaDeploymentParameters = camundaDeploymentParameters;
            this.operationService = operationService;
            this.syrinxCamundaClientService = syrinxCamundaClientService;
        }

        public void Upgrade()
        {
            var context = OperationContext.Builder()
                .SetName(GetType().FullName, nameof(Upgrade))
                .SetUserId(null, true)
                .Create();

            operationService.Make(context, async operation =>
            {
                var logger = operation.Logger<CamundaDeploymentService>();

                await logger.Info("Upgrading to latest version of Camunda workflows is started");
                var settings = GetSettings(operation);
                var files = GetFiles(operation);

                for (var i = 0; i < files.Count; i++)
                {
                    await logger.Info($"– [{i + 1}] Camunda workflow file: {files[i].FileName}");
                }

                var response = await new Deployment.CreateRequest
                {
                    DeploymentName = settings.VersionName,
                    DeploymentSource = settings.ProjectName,
                    EnableDuplicateFiltering = true,
                    DeployChangedOnly = true,
                    Files = files
                }.SendRequest(operation, syrinxCamundaClientService);

                if (response.IsWithoutErrors())
                {
                    await logger.Info($"Camunda deployment was created:\n{JsonConvert.SerializeObject(response.Response, Formatting.Indented)}");
                }
                else
                {
                    await logger.Error($"Camunda deployment wasn't created:\n{JsonConvert.SerializeObject(response.Errors, Formatting.Indented)}");
                    throw CommonExceptions.FailedToDeployCamundaWorkflows(operation);
                }
            }).Wait();
        }

        public void DowngradeToPrevious()
        {
            var context = OperationContext.Builder()
                .SetName(GetType().FullName, nameof(DowngradeToPrevious))
                .SetUserId(null, true)
                .Create();

            operationService.Make(context, async operation =>
            {
                var logger = operation.Logger<CamundaDeploymentService>();

                await logger.Info("Downgrading to previous deployment of Camunda workflows is started");
                var settings = GetSettings(operation);

                var deployments = await GetDeployments(operation, logger, settings);
                if (deployments.Count > 0)
                {
                    await DeleteDeployment(operation, logger, deployments[0]);
                }
            }).Wait();
        }

        public void Downgrade()
        {
            var context = OperationContext.Builder()
                .SetName(GetType().FullName, nameof(Downgrade))
                .SetUserId(null, true)
                .Create();

            operationService.Make(context, async operation =>
            {
                var logger = operation.Logger<CamundaDeploymentService>();

                await logger.Info("Downgrading of all Camunda workflows is started");
                var settings = GetSettings(operation);

                var deployments = await GetDeployments(operation, logger, settings);
                foreach (var deployment in deployments)
                {
                    await DeleteDeployment(operation, logger, deployment);
                }
            }).Wait();
        }

        private async Task<List<DeploymentListItem>> GetDeployments(IOperation operation, IOperationLogger logger, CamundaDeploymentSettings settings)
        {
            var response = await new Deployment.GetListRequest
            {
                Source = settings.ProjectName,
                SortBy = "deploymentTime",
                SortOrder = "desc"
            }.SendRequest(operation, syrinxCamundaClientService);

            if (response.IsWithoutErrors())
            {
                await logger.Info($"Camunda deployments were got successfully:\n{JsonConvert.SerializeObject(response.Response, Formatting.Indented)}");
                return response.Response;
            }

            await logger.Error($"Camunda deployments weren't got:\n{JsonConvert.SerializeObject(response.Errors, Formatting.Indented)}");
            throw CommonExceptions.FailedToGetListOfCamundaDeployments(operation);
        }

        private async Task DeleteDeployment(IOperation operation, IOperationLogger logger, DeploymentListItem deployment)
        {
            var response = await new Deployment.DeleteRequest(deployment.Id)
            {
                Cascade = true
            }.SendRequest(operation, syrinxCamundaClientService);

            if (response.IsWithoutErrors())
            {
                await logger.Info($"Camunda deployment was deleted:\n{JsonConvert.SerializeObject(deployment, Formatting.Indented)}");
            }
            else
            {
                await logger.Error($"Camunda deployments wasn't deleted:\n{JsonConvert.SerializeObject(response.Errors, Formatting.Indented)}");
                throw CommonExceptions.FailedToDeleteCamundaDeployment(operation);
            }
        }
    }
}