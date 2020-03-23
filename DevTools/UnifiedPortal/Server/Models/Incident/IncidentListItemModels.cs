using System;
using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.Incident;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Common;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.SSP;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Incident
{
    public class IncidentListItemModel : PagingResponseItemModel, IPagingResponseItemModel<IncidentListItemModel, IncidentListItem>
    {
        public long OperationId { get; set; }
        public string Scope { get; set; }
        public string ContextName { get; set; }

        public int AuthorId { get; set; }
        public string AuthorColor { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }

        public int? AssigneeId { get; set; }
        public string AssigneeColor { get; set; }
        public string AssigneeFirstName { get; set; }
        public string AssigneeLastName { get; set; }

        public DateTime InitialTime { get; set; }
        public IncidentStatus Status { get; set; }

        public IncidentListItemModel ToModel(IncidentListItem entity)
        {
            OperationId = entity.OperationId;
            Scope = entity.Scope;
            ContextName = entity.ContextName;
            AuthorId = entity.AuthorId;
            AuthorColor = entity.AuthorColor;
            AuthorFirstName = entity.AuthorFirstName;
            AuthorLastName = entity.AuthorLastName;
            AssigneeId = entity.AssigneeId;
            AssigneeColor = entity.AssigneeColor;
            AssigneeFirstName = entity.AssigneeFirstName;
            AssigneeLastName = entity.AssigneeLastName;
            InitialTime = entity.InitialTime;
            Status = entity.Status;
            return this;
        }
    }

    public class IncidentPagingFilterModel : PagingFilterModel, IPagingFilterModel<IncidentPagingFilter>
    {
        public IEnumerable<int> OperationIds { get; set; }
        public IEnumerable<int> AuthorIds { get; set; }
        public IEnumerable<int> AssigneeIds { get; set; }
        public RangeFilterModel<DateTime> InitialTimeRange { get; set; }
        public IEnumerable<IncidentStatus> Statuses { get; set; }

        public IncidentPagingFilter ToEntity() => new IncidentPagingFilter
        {
            OperationIds = OperationIds,
            AuthorIds = AuthorIds,
            AssigneeIds = AssigneeIds,
            InitialTimeRange = InitialTimeRange.ToEntity(),
            Statuses = Statuses
        };
    }

    public class IncidentPagingRequestModel : ServerSidePagingRequestModel<IncidentPagingFilterModel, IncidentPagingRequest>
    {
        public override IncidentPagingRequest ToEntity() => new IncidentPagingRequest
        {
            PageNo = PageNo,
            PageSize = PageSize,
            Filter = Filter.ToEntity()
        };
    }

    public class IncidentPagingResponseModel : ServerSidePagingResponseModel<IncidentListItemModel, IncidentPagingResponseModel, IncidentPagingResponse>
    {
        public override IncidentPagingResponseModel ToModel(IncidentPagingResponse entity)
        {
            Total = entity.Total;
            Items = entity.Items.Select(item => new IncidentListItemModel().ToModel(item));
            return this;
        }
    }
}