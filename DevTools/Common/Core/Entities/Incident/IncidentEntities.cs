using System;
using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.SSP;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Incident
{
    public class IncidentBaseEntity
    {
        public long OperationId { get; set; }
        public int AuthorId { get; set; }
        public int? AssigneeId { get; set; }
        public IncidentStatus Status { get; set; }
    }

    public class IncidentUpdateEntity : IncidentBaseEntity
    {
        public string Comment { get; set; }
    }

    public class IncidentEntity : IncidentBaseEntity
    {
        public string Scope { get; set; }
        public string ContextName { get; set; }

        public string AuthorColor { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }

        public string AssigneeColor { get; set; }
        public string AssigneeFirstName { get; set; }
        public string AssigneeLastName { get; set; }

        public DateTime InitialTime { get; set; }
    }

    public class IncidentListItem : IncidentEntity, IPagingResponseItemEntity
    {
    }

    public class IncidentPagingFilter : IPagingFilterEntity
    {
        public IEnumerable<int> OperationIds { get; set; }
        public IEnumerable<int> AuthorIds { get; set; }
        public IEnumerable<int> AssigneeIds { get; set; }
        public RangeFilter<DateTime> InitialTimeRange { get; set; }
        public IEnumerable<IncidentStatus> Statuses { get; set; }
    }

    public class IncidentPagingRequest : ServerSidePagingRequest<IncidentPagingFilter>
    {
    }

    public class IncidentPagingResponse : ServerSidePagingResponse<IncidentListItem>
    {
    }

    public class IncidentFullEntity : IncidentEntity
    {
        public IEnumerable<IncidentHistoryEntity> History { get; set; }
    }
}