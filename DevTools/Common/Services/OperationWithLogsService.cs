using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Incident;
using VXDesign.Store.DevTools.Common.Core.Entities.Operation;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Entities;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface IOperationWithLogsService
    {
        Task<OperationPagingResponse> GetItems(IOperation operation, OperationPagingRequest request);

        #region Incident

        Task<IncidentFullEntity> GetIncidentWithHistory(IOperation operation, long operationId);
        Task InitializeIncident(IOperation operation, IncidentUpdateEntity entity);
        Task SaveComment(IOperation operation, long operationId, int? userId, int? historyId, string comment);
        Task DeleteComment(IOperation operation, long operationId, long historyId);
        Task UpdateIncident(IOperation operation, IncidentUpdateEntity entity);

        #endregion
    }

    public class OperationWithLogsService : IOperationWithLogsService
    {
        private readonly IOperationStore operationStore;
        private readonly ILoggerStore loggerStore;
        private readonly IIncidentStore incidentStore;

        public OperationWithLogsService(IOperationStore operationStore, ILoggerStore loggerStore, IIncidentStore incidentStore)
        {
            this.operationStore = operationStore;
            this.loggerStore = loggerStore;
            this.incidentStore = incidentStore;
        }

        public async Task<OperationPagingResponse> GetItems(IOperation operation, OperationPagingRequest request)
        {
            var (total, operations) = await operationStore.Get(operation, request);
            var operationList = operations.ToList();
            var logs = await loggerStore.GetByOperations(operationList.Select(item => item.Id));
            return new OperationPagingResponse
            {
                Total = total,
                Items = PreparePagingItems(operationList, logs)
            };
        }

        private static IEnumerable<OperationWithLogs> PreparePagingItems(IEnumerable<OperationEntity> operations, IEnumerable<LogEntity> logs)
        {
            return operations.Select(operation => new OperationWithLogs
            {
                Operation = operation,
                Logs = logs.Where(log => log.OperationId == operation.Id).ToList()
            });
        }

        public async Task<IncidentFullEntity> GetIncidentWithHistory(IOperation operation, long operationId)
        {
            if (!await incidentStore.IsIncidentExist(operation, operationId))
            {
                throw CommonExceptions.IncidentWasNotFound(operation, operationId);
            }

            var incident = await incidentStore.GetIncidentWithHistory(operation, operationId);
            incident.Logs = await loggerStore.GetByOperations(new[] { operationId });
            return incident;
        }

        public async Task InitializeIncident(IOperation operation, IncidentUpdateEntity entity)
        {
            if (await incidentStore.IsIncidentExist(operation, entity.IncidentOperationId))
            {
                throw CommonExceptions.IncidentAlreadyExists(operation, entity.IncidentOperationId);
            }

            if (!await operationStore.IsOperationExist(operation, entity.IncidentOperationId))
            {
                throw CommonExceptions.OperationWasNotFound(operation, entity.IncidentOperationId);
            }

            await incidentStore.InitializeIncident(operation, entity);
        }

        public async Task SaveComment(IOperation operation, long operationId, int? userId, int? historyId, string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                throw CommonExceptions.IncidentCommentIsEmpty(operation);
            }

            if (!await incidentStore.IsIncidentExist(operation, operationId))
            {
                throw CommonExceptions.IncidentWasNotFound(operation, operationId);
            }

            await incidentStore.SaveComment(operation, operationId, userId, historyId, comment);
        }

        public async Task DeleteComment(IOperation operation, long operationId, long historyId)
        {
            if (!await incidentStore.IsIncidentExist(operation, operationId))
            {
                throw CommonExceptions.IncidentWasNotFound(operation, operationId);
            }

            await incidentStore.DeleteComment(operation, historyId);
        }

        public async Task UpdateIncident(IOperation operation, IncidentUpdateEntity entity)
        {
            if (!await incidentStore.IsIncidentExist(operation, entity.IncidentOperationId))
            {
                throw CommonExceptions.IncidentWasNotFound(operation, entity.IncidentOperationId);
            }

            var previous = await incidentStore.GetIncident(operation, entity.IncidentOperationId);
            var history = new IncidentHistoryRecordEntity();

            if (previous.AuthorId != entity.AuthorId)
            {
                history.AuthorId = entity.AuthorId;
            }

            if (previous.AssigneeId != entity.AssigneeId)
            {
                if (entity.AssigneeId == null)
                {
                    history.IsUnassigned = true;
                }

                if (previous.AssigneeId == null)
                {
                    history.IsUnassigned = false;
                }

                history.AssigneeId = entity.AssigneeId;
            }

            if (previous.Status != entity.Status)
            {
                history.Status = entity.Status;
            }

            await incidentStore.UpdateIncident(operation, entity, history);
        }
    }
}