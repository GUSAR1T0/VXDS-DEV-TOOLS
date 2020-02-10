using System;
using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Core.Entities.Storage.Log;
using VXDesign.Store.DevTools.UnifiedPortal.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Models.Common;
using VXDesign.Store.DevTools.UnifiedPortal.Models.SSP;

namespace VXDesign.Store.DevTools.UnifiedPortal.Models.Log
{
    public class OperationWithLogsModel : PagingResponseItemModel, IPagingResponseItemModel<OperationWithLogsModel, OperationWithLogs>
    {
        private const string DateFormat = "dddd, dd MMMM yyyy, HH:mm:ss.fff";

        public long Id { get; set; }
        public string Scope { get; set; }
        public string ContextName { get; set; }
        public int? UserId { get; set; }
        public string Color { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsSuccessful { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        public IEnumerable<LogModel> Logs { get; set; }

        public OperationWithLogsModel ToModel(OperationWithLogs entity) => new OperationWithLogsModel
        {
            Id = entity.Operation.Id,
            Scope = entity.Operation.Scope,
            ContextName = entity.Operation.ContextName,
            UserId = entity.Operation.UserId,
            Color = entity.Operation.Color,
            FirstName = !entity.Operation.IsSystemAction ? entity.Operation.FirstName ?? "Unauthorized" : "System",
            LastName = entity.Operation.LastName,
            IsSuccessful = entity.Operation.IsSuccessful,
            StartTime = entity.Operation.StartTime.ToString(DateFormat),
            StopTime = entity.Operation.StopTime?.ToString(DateFormat),
            Logs = entity.Logs.Select(log => log.ToModel())
        };
    }

    public class OperationPagingFilterModel : PagingFilterModel, IPagingFilterModel<OperationPagingFilter>
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> Scopes { get; set; }
        public IEnumerable<string> ContextNames { get; set; }
        public IEnumerable<int> UserIds { get; set; }
        public bool? IsSystemAction { get; set; }
        public bool? IsSuccessful { get; set; }
        public RangeFilterModel<DateTime> StartTimeRange { get; set; }
        public RangeFilterModel<DateTime> StopTimeRange { get; set; }

        public OperationPagingFilter ToEntity() => new OperationPagingFilter
        {
            Ids = Ids,
            Scopes = Scopes,
            ContextNames = ContextNames,
            UserIds = UserIds,
            IsSystemAction = IsSystemAction,
            IsSuccessful = IsSuccessful,
            StartTimeRange = StartTimeRange.ToEntity(),
            StopTimeRange = StopTimeRange.ToEntity()
        };
    }

    public class OperationPagingRequestModel : ServerSidePagingRequestModel<OperationPagingFilterModel, OperationPagingRequest>
    {
        public override OperationPagingRequest ToEntity() => new OperationPagingRequest
        {
            PageNo = PageNo,
            PageSize = PageSize,
            Filter = Filter.ToEntity()
        };
    }

    public class OperationPagingResponseModel : ServerSidePagingResponseModel<OperationWithLogsModel, OperationPagingResponseModel, OperationPagingResponse>
    {
        public override OperationPagingResponseModel ToModel(OperationPagingResponse entity)
        {
            Total = entity.Total;
            Items = entity.Items.Select(item => new OperationWithLogsModel().ToModel(item));
            return this;
        }
    }
}