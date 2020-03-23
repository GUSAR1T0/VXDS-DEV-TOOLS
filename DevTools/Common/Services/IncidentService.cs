using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Incident;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface IIncidentService
    {
        Task<IncidentPagingResponse> GetItems(IOperation operation, IncidentPagingRequest request);
        Task<IncidentFullEntity> GetIncidentWithHistory(IOperation operation, long operationId);
        Task InitializeIncident(IOperation operation, IncidentUpdateEntity entity);
        Task UpdateIncident(IOperation operation, IncidentUpdateEntity entity);
    }

    public class IncidentService : IIncidentService
    {
        private readonly IIncidentStore incidentStore;

        public IncidentService(IIncidentStore incidentStore)
        {
            this.incidentStore = incidentStore;
        }

        public async Task<IncidentPagingResponse> GetItems(IOperation operation, IncidentPagingRequest request)
        {
            var (total, incidents) = await incidentStore.GetIncidents(operation, request);
            return new IncidentPagingResponse
            {
                Total = total,
                Items = incidents
            };
        }

        public async Task<IncidentFullEntity> GetIncidentWithHistory(IOperation operation, long operationId) => await incidentStore.GetIncidentWithHistory(operation, operationId);

        public async Task InitializeIncident(IOperation operation, IncidentUpdateEntity entity)
        {
            if (await incidentStore.IsIncidentExist(operation, entity.OperationId))
            {
                throw CommonExceptions.IncidentAlreadyExists(operation, entity.OperationId);
            }

            await incidentStore.InitializeIncident(operation, entity);
        }

        public async Task UpdateIncident(IOperation operation, IncidentUpdateEntity entity)
        {
            if (!await incidentStore.IsIncidentExist(operation, entity.OperationId))
            {
                throw CommonExceptions.IncidentWasNotFound(operation, entity.OperationId);
            }

            var previous = await incidentStore.GetIncident(operation, entity.OperationId);
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