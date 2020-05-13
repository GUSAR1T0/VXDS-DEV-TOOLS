using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Config;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Entities;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Exceptions;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Models.ExternalTask;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Core.Utils;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Base
{
    public class CamundaWorkers<TProperties> where TProperties : CamundaWorkersProperties, new()
    {
        public class CamundaWorkersBuilder
        {
            internal string LogScope { get; private set; }
            internal TProperties Properties { get; private set; }
            internal ISyrinxCamundaClientService CamundaClient { get; private set; }
            internal List<Func<CamundaWorkers<TProperties>, Task>> RunnableTasks { get; } = new List<Func<CamundaWorkers<TProperties>, Task>>();
            internal IServiceCollection ServiceCollection { get; } = new ServiceCollection();

            internal CamundaWorkersBuilder()
            {
            }

            public CamundaWorkersBuilder SetProperties(Func<IConfiguration> configuration)
            {
                Properties = PropertiesUtils.Create<TProperties>(configuration()) ?? throw CamundaWorkersBuilderException.PropertiesAreEmpty();
                CamundaClient = new SyrinxCamundaClientService(Properties.SyrinxProperties);
                ServiceCollection.AddScoped(factory => CamundaClient);
                return this;
            }

            public CamundaWorkersBuilder SetLogger(Func<LoggingConfiguration> configuration, string scope)
            {
                LogManager.Configuration = configuration();
                LogScope = !string.IsNullOrWhiteSpace(scope) ? scope : throw CamundaWorkersBuilderException.LogScopeIsNotStated();
                return this;
            }

            public CamundaWorkersBuilder Configure(Action<IServiceCollection> action)
            {
                action(ServiceCollection);
                return this;
            }

            public CamundaWorkersBuilder AddWorker<TCamundaWorker>() where TCamundaWorker : CamundaWorker
            {
                var camundaWorkerType = typeof(TCamundaWorker);
                var operationContext = OperationContext.Builder()
                    .SetName(camundaWorkerType.FullName, GetTopicName(camundaWorkerType), "Background")
                    .SetUserId(null, true)
                    .SetIsolationLevel(IsolationLevel.Unspecified)
                    .Create();
                RunnableTasks.Add(workers => workers.OperationService.Make(operationContext, workers.Fetch<TCamundaWorker>));
                ServiceCollection.AddScoped<TCamundaWorker>();
                return this;
            }

            public CamundaWorkers<TProperties> Launch()
            {
                try
                {
                    var workers = new CamundaWorkers<TProperties>(this);
                    Task.WaitAll(workers.RunnableTasks.Select(task => task(workers)).ToArray());
                    return workers;
                }
                finally
                {
                    LogManager.Shutdown();
                }
            }
        }

        public static CamundaWorkersBuilder Builder() => new CamundaWorkersBuilder();
        private static string GetTopicName(Type type) => type.GetCustomAttributes<CamundaWorkerTopicAttribute>(false).FirstOrDefault()?.Name ?? type.Name;

        private TProperties Properties { get; }
        private IOperationService OperationService { get; }
        private ISyrinxCamundaClientService CamundaClient { get; }
        private List<Func<CamundaWorkers<TProperties>, Task>> RunnableTasks { get; }
        private IServiceCollection ServiceCollection { get; }

        internal CamundaWorkers(CamundaWorkersBuilder builder)
        {
            Properties = builder.Properties;
            var loggerStore = new LoggerStore(Properties.DatabaseConnectionProperties.LogStoreConnectionString, builder.LogScope);
            OperationService = new OperationService(loggerStore, Properties.DatabaseConnectionProperties.DataStoreConnectionString, builder.LogScope);
            CamundaClient = builder.CamundaClient;
            RunnableTasks = builder.RunnableTasks;
            ServiceCollection = builder.ServiceCollection;
        }

        private async Task Fetch<TCamundaWorker>(IOperation operation) where TCamundaWorker : CamundaWorker
        {
            var logger = operation.Logger<TCamundaWorker>();

            var topics = new CamundaTopics
            {
                new CamundaTopic
                {
                    TopicName = GetTopicName(typeof(TCamundaWorker)),
                    LockDuration = Properties.LockDuration,
                    Variables = CamundaWorker.InputVariableNames(typeof(TCamundaWorker))
                }
            };

            var retries = Properties.CountOfRetriesWhenFetchIsUnsuccessful;
            while (retries > 0)
            {
                await logger.Debug("Fetching task");

                var response = await new ExternalTask.FetchAndLockRequest
                {
                    AsyncResponseTimeout = Properties.FetchTimeout,
                    UsePriority = Properties.UsePriority,
                    MaxTasks = 1,
                    WorkerId = $"w-{Properties.WorkerKeyword}-{DateTime.Today:yyyyMMdd}",
                    Topics = topics
                }.SendRequest(operation, CamundaClient);

                if (!response.IsWithoutErrors())
                {
                    retries--;
                    var retryAfter = TimeSpan.FromMilliseconds(Properties.RetryAfterFetchTimeout);
                    await logger.Error($"Failed to fetch task, retry after {retryAfter}... (remaining attempts: {retries})", response.ToLog());
                    if (retries != 0) await Task.Delay(retryAfter);
                }
                else if (response.Response.Count > 0)
                {
                    await logger.Debug("Executing task");

                    try
                    {
                        var camundaWorkerType = typeof(TCamundaWorker);
                        var operationContext = OperationContext.Builder()
                            .SetName(camundaWorkerType.FullName, GetTopicName(camundaWorkerType), "TaskRunner")
                            .SetUserId(null, true)
                            .Create();
                        await OperationService.Make(operationContext, async innerOperation => await Run<TCamundaWorker>(innerOperation, logger, response.Response[0]));
                    }
                    catch
                    {
                        // ignored
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        private async Task Run<TCamundaWorker>(IOperation operation, IOperationLogger logger, LockedExternalTaskListItem item) where TCamundaWorker : CamundaWorker
        {
            using var scope = ServiceCollection.BuildServiceProvider(false).CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<TCamundaWorker>();
            worker.InitializeVariables(item.Variables);

            Exception exception = null;
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                try
                {
                    var token = cancellationTokenSource.Token;
#pragma warning disable 4014
                    Task.Run(async () => await ExtendLock(operation, logger, item, token), token);
#pragma warning restore 4014
                    worker.Execute(operation, logger);
                }
                catch (CamundaWorkerExecutionIsNotCompletedYet)
                {
                    await logger.Debug("Skipping task");
                    return;
                }
                catch (CamundaWorkerBpmnError error)
                {
                    await logger.Debug("BPMN error is thrown");
                    exception = await ThrowBpmnError(operation, logger, item, worker, error);
                    if (exception.IsEmpty()) return;
                }
                catch (Exception e)
                {
                    await logger.Error("Failed to perform task", e);
                    exception = e;
                }
                finally
                {
                    cancellationTokenSource.Cancel(true);
                }
            }

            var retries = Properties.CountOfRetriesWhenFailuresAre;
            while (retries > 0)
            {
                var retryAfter = TimeSpan.FromMilliseconds(Properties.RetryAfterFailureTimeout);

                IntermediateCamundaResponse<EmptyResult> response;
                if (exception.IsEmpty())
                {
                    await logger.Trace("Trying to complete task");
                    response = await new ExternalTask.CompleteRequest(item.Id)
                    {
                        WorkerId = item.WorkerId,
                        Variables = worker.CollectVariables()
                    }.SendRequest(operation, CamundaClient);
                }
                else
                {
                    var countOfRetries = (item.Retries ?? Properties.CountOfRetriesWhenFailuresAre) - 1;
                    await logger.Trace($"Trying to handle failure, retry task execution after {retryAfter}... (remaining attempts: {countOfRetries})");
                    response = await new ExternalTask.HandleFailureRequest(item.Id)
                    {
                        WorkerId = item.WorkerId,
                        ErrorMessage = exception?.Message,
                        ErrorDetails = exception?.StackTrace,
                        Retries = countOfRetries,
                        RetryTimeout = Properties.RetryAfterFailureTimeout
                    }.SendRequest(operation, CamundaClient);
                }

                if (!response.IsWithoutErrors())
                {
                    retries--;
                    var resultType = exception.IsEmpty() ? "complete task" : "handle failure";
                    await logger.Error($"Couldn't {resultType}, retry task handling after {retryAfter}... (remaining attempts: {retries})", response.ToLog());
                    if (retries != 0) await Task.Delay(retryAfter);
                }
                else
                {
                    await logger.Debug($"{(exception.IsEmpty() ? "Complete task" : "Handle failure")} process is successful");
                    break;
                }
            }

            if (retries == 0)
            {
                await logger.Fatal($"Couldn't {(exception.IsEmpty() ? "complete task" : "handle failure")}, stop retrying");
            }

            if (!exception.IsEmpty())
            {
                throw exception;
            }
        }

        private async Task ExtendLock(IOperation operation, IOperationLogger logger, LockedExternalTaskListItem item, CancellationToken token)
        {
            while (true)
            {
                try
                {
                    while (true)
                    {
                        await logger.Debug("Extending lock");

                        var response = await new ExternalTask.ExtendLockRequest(item.Id)
                        {
                            WorkerId = item.WorkerId,
                            NewDuration = Properties.LockDuration
                        }.SendRequest(operation, CamundaClient);

                        if (!response.IsWithoutErrors())
                        {
                            var retryAfter = TimeSpan.FromSeconds(5);
                            await logger.Error($"Couldn't extend lock for task, retry after {retryAfter}", response.ToLog());
                            await Task.Delay(retryAfter, token);
                        }
                        else
                        {
                            await logger.Debug("Extend lock process is successful");
                            await Task.Delay(TimeSpan.FromMilliseconds(Properties.LockDuration).Divide(2), token);
                            break;
                        }
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        private async Task<Exception> ThrowBpmnError<TCamundaWorker>(IOperation operation, IOperationLogger logger, LockedExternalTaskListItem item, TCamundaWorker worker,
            CamundaWorkerBpmnError error) where TCamundaWorker : CamundaWorker
        {
            try
            {
                var retries = Properties.CountOfRetriesWhenFailuresAre;
                var retryAfter = TimeSpan.FromMilliseconds(Properties.RetryAfterFailureTimeout);
                ExternalTask.HandleBpmnErrorResponse response = null;
                while (retries > 0)
                {
                    response = await new ExternalTask.HandleBpmnErrorRequest(item.Id)
                    {
                        WorkerId = item.WorkerId,
                        ErrorCode = error.Code,
                        ErrorMessage = error.Message,
                        Variables = worker.CollectVariables()
                    }.SendRequest(operation, CamundaClient);

                    if (!response.IsWithoutErrors())
                    {
                        retries--;
                        await logger.Error($"Couldn't send BPMN error for task, retry task handling after {retryAfter}... (remaining attempts: {retries})", response.ToLog());
                        if (retries != 0) await Task.Delay(retryAfter);
                    }
                    else
                    {
                        await logger.Debug("Send BPMN error is successful");
                        break;
                    }
                }

                if (retries == 0)
                {
                    await logger.Fatal("Couldn't send BPMN error for task, stop retrying");
                    return new Exception(response?.Output);
                }
            }
            catch (Exception e)
            {
                await logger.Error("Failed to send BPMN error for task", e);
                return e;
            }

            return null;
        }
    }
}