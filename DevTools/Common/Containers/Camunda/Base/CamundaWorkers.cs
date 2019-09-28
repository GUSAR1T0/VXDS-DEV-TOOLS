using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;
using VXDesign.Store.DevTools.Common.Attributes;
using VXDesign.Store.DevTools.Common.Containers.Camunda.ExternalTask.Entities;
using VXDesign.Store.DevTools.Common.Containers.Properties;
using VXDesign.Store.DevTools.Common.Entities.Enums;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
using VXDesign.Store.DevTools.Common.Extensions.Base;
using VXDesign.Store.DevTools.Common.Extensions.HTTP;
using VXDesign.Store.DevTools.Common.Services.Syrinx;
using VXDesign.Store.DevTools.Common.Utils.Properties;

namespace VXDesign.Store.DevTools.Common.Containers.Camunda.Base
{
    public abstract class CamundaWorker
    {
        private class VariableProperty
        {
            internal PropertyInfo Property { get; set; }
            internal CamundaWorkerVariableAttribute Attribute { get; set; }
            internal string GetVariableName => Attribute.Name ?? Property.Name;
        }

        private static IEnumerable<VariableProperty> GetVariablePropertiesAndAttributes(Type type, CamundaVariableDirection direction) => type
            .GetProperties()
            .Select(property => new VariableProperty
            {
                Property = property,
                Attribute = property.GetCustomAttributes<CamundaWorkerVariableAttribute>(false).FirstOrDefault()
            })
            .Where(arg => arg.Attribute != null && arg.Attribute.Direction.HasFlag(direction))
            .ToList();

        internal static IEnumerable<string> InputVariableNames(Type type) => GetVariablePropertiesAndAttributes(type, CamundaVariableDirection.Input).Select(variable => variable.GetVariableName);

        internal void InitializeVariables(IReadOnlyCamundaVariables variables)
        {
            var properties = GetVariablePropertiesAndAttributes(GetType(), CamundaVariableDirection.Input);
            foreach (var property in properties)
            {
                if (variables.ContainsKey(property.GetVariableName))
                {
                    var variable = variables[property.GetVariableName];
                    property.Property.SetPropertyValue(this, variable?.To(property.Property.PropertyType));
                }
            }
        }

        internal ICamundaVariables CollectVariables()
        {
            var properties = GetVariablePropertiesAndAttributes(GetType(), CamundaVariableDirection.Output);
            var variables = new CamundaVariables();
            foreach (var property in properties)
            {
                var variableName = property.GetVariableName;
                var propertyInfo = property.Property;
                var variableValue = propertyInfo.GetValue(this);

                if (propertyInfo.PropertyType == typeof(bool) || propertyInfo.PropertyType == typeof(bool?))
                {
                    variables.Add(variableName, (bool?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(byte[]))
                {
                    variables.Add(variableName, (byte[]) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(short) || propertyInfo.PropertyType == typeof(short?))
                {
                    variables.Add(variableName, (short?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                {
                    variables.Add(variableName, (int?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(long) || propertyInfo.PropertyType == typeof(long?))
                {
                    variables.Add(variableName, (long?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(double?))
                {
                    variables.Add(variableName, (double?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(decimal) || propertyInfo.PropertyType == typeof(decimal?))
                {
                    variables.Add(variableName, (decimal?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                {
                    variables.Add(variableName, (DateTime?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(DateTimeOffset) || propertyInfo.PropertyType == typeof(DateTimeOffset?))
                {
                    variables.Add(variableName, (DateTimeOffset?) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    variables.Add(variableName, (string) variableValue);
                }
                else if (propertyInfo.PropertyType == typeof(CamundaFile))
                {
                    variables.Add(variableName, (CamundaFile) variableValue);
                }
                else
                {
                    variables.Add(property.GetVariableName, propertyInfo.GetValue(this));
                }
            }

            return variables;
        }

        public abstract void Execute(ILogger logger);
    }

    public class CamundaWorkers<TProperties> where TProperties : CamundaWorkersProperties, new()
    {
        public class CamundaWorkersBuilder
        {
            internal TProperties Properties { get; private set; }
            internal List<Func<CamundaWorkers<TProperties>, Task>> RunnableTasks { get; } = new List<Func<CamundaWorkers<TProperties>, Task>>();

            internal CamundaWorkersBuilder()
            {
            }

            public CamundaWorkersBuilder SetProperties(Func<IConfiguration> configuration)
            {
                Properties = PropertiesCreator.Create<TProperties>(configuration());
                return this;
            }

            public CamundaWorkersBuilder SetLogger(Func<LoggingConfiguration> configuration)
            {
                LogManager.Configuration = configuration();
                return this;
            }

            public CamundaWorkersBuilder SetWorker<TCamundaWorker>() where TCamundaWorker : CamundaWorker, new()
            {
                RunnableTasks.Add(workers => workers.Fetch<TCamundaWorker>());
                return this;
            }

            public CamundaWorkers<TProperties> Create()
            {
                var workers = new CamundaWorkers<TProperties>(this);
                Task.WaitAll(workers.RunnableTasks.Select(task => task(workers)).ToArray());
                return workers;
            }
        }

        public static CamundaWorkersBuilder Builder() => new CamundaWorkersBuilder();

        internal TProperties Properties { get; }
        private ISyrinxClientService Service { get; }
        internal List<Func<CamundaWorkers<TProperties>, Task>> RunnableTasks { get; }

        internal CamundaWorkers(CamundaWorkersBuilder builder)
        {
            Properties = builder.Properties;
            Service = new SyrinxClientService(Properties.SyrinxProperties);
            RunnableTasks = builder.RunnableTasks;
        }

        private async Task Fetch<TCamundaWorker>() where TCamundaWorker : CamundaWorker, new()
        {
            var logger = LogManager.GetLogger(typeof(TCamundaWorker).FullName);

            var topics = new CamundaTopics
            {
                new CamundaTopic
                {
                    TopicName = typeof(TCamundaWorker).GetCustomAttributes<CamundaWorkerTopicAttribute>(false).FirstOrDefault()?.Name,
                    LockDuration = Properties.LockDuration,
                    Variables = CamundaWorker.InputVariableNames(typeof(TCamundaWorker))
                }
            };

            while (true)
            {
                logger.Debug("Fetching task");

                var response = await new ExternalTask.Models.ExternalTask.FetchAndLockRequest
                {
                    AsyncResponseTimeout = Properties.FetchTimeout,
                    UsePriority = Properties.UsePriority,
                    MaxTasks = 1,
                    WorkerId = $"w-{Properties.WorkerKeyword}-{DateTime.Today:yyyyMMddhhmmss}",
                    Topics = topics
                }.SendRequest(Service);

                if (!response.IsWithoutErrors())
                {
                    logger.Error("Failed to fetch task");
                }
                else if (response.Response.Count > 0)
                {
                    logger.Debug("Executing task");
                    await Run<TCamundaWorker>(logger, response.Response[0]);
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private async Task Run<TCamundaWorker>(ILogger logger, LockedExternalTaskListItem item) where TCamundaWorker : CamundaWorker, new()
        {
            var worker = new TCamundaWorker();
            worker.InitializeVariables(item.Variables);

            bool isSuccess;
            Exception exception = null;
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                try
                {
                    var token = cancellationTokenSource.Token;
#pragma warning disable 4014
                    Task.Run(async () => await ExtendLock(logger, item, token), token);
#pragma warning restore 4014
                    worker.Execute(logger);
                    isSuccess = true;
                }
                catch (CamundaWorkerExecutionIsNotCompletedYet)
                {
                    logger.Debug("Skipping task");
                    return;
                }
                catch (Exception e)
                {
                    logger.Fatal("Failed to perform task");
                    exception = e;
                    isSuccess = false;
                }
                finally
                {
                    cancellationTokenSource.Cancel(true);
                }
            }

            var retries = 5;
            while (retries > 0)
            {
                IntermediateCamundaResponse<EmptyResult> response;
                if (isSuccess)
                {
                    response = await new ExternalTask.Models.ExternalTask.CompleteRequest(item.Id)
                    {
                        WorkerId = item.WorkerId,
                        Variables = worker.CollectVariables()
                    }.SendRequest(Service);
                }
                else
                {
                    var countOfRetries = (item.Retries ?? Properties.CountOfRetriesWhenFailuresAre) - 1;
                    response = await new ExternalTask.Models.ExternalTask.HandleFailureRequest(item.Id)
                    {
                        WorkerId = item.WorkerId,
                        ErrorMessage = exception.Message,
                        ErrorDetails = exception.StackTrace,
                        Retries = countOfRetries,
                        RetryTimeout = Properties.RetryAfterFailureTimeout
                    }.SendRequest(Service);
                }

                if (!response.IsWithoutErrors())
                {
                    retries--;
                    logger.Error($"Couldn't {(isSuccess ? "complete task" : "handle failure")}, retry... (remaining attempts: {retries})");
                }
                else
                {
                    logger.Debug($"{(isSuccess ? "Complete task" : "Handle failure")} process is successful");
                    break;
                }
            }

            if (retries == 0)
            {
                logger.Error($"Couldn't {(isSuccess ? "complete task" : "handle failure")}, stop retrying");
            }
        }

        private async Task ExtendLock(ILogger logger, LockedExternalTaskListItem item, CancellationToken token)
        {
            while (true)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(Properties.LockDuration).Divide(2), token);

                    while (true)
                    {
                        logger.Debug("Extending lock");

                        var response = await new ExternalTask.Models.ExternalTask.ExtendLockRequest(item.Id)
                        {
                            WorkerId = item.WorkerId,
                            NewDuration = Properties.LockDuration
                        }.SendRequest(Service);

                        if (!response.IsWithoutErrors())
                        {
                            logger.Error("Couldn't extend lock for task");
                            await Task.Delay(TimeSpan.FromSeconds(5), token);
                        }
                        else
                        {
                            logger.Debug("Extend lock process is successful");
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
    }
}