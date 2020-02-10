using System;
using System.Collections.Generic;
using VXDesign.Store.DevTools.Core.Entities.Storage.SSP;

namespace VXDesign.Store.DevTools.Core.Entities.Storage.Log
{
    public class OperationEntity : IDataEntity
    {
        public long Id { get; set; }
        public string Scope { get; set; }
        public string ContextName { get; set; }
        public int? UserId { get; set; }
        public string Color { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSystemAction { get; set; }
        public bool? IsSuccessful { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
    }

    public class OperationWithLogs : IPagingResponseItemEntity
    {
        public OperationEntity Operation { get; set; }
        public IEnumerable<LogEntity> Logs { get; set; }
    }

    public class OperationPagingFilter : IPagingFilterEntity
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> Scopes { get; set; }
        public IEnumerable<string> ContextNames { get; set; }
        public IEnumerable<int> UserIds { get; set; }
        public bool? IsSystemAction { get; set; }
        public bool? IsSuccessful { get; set; }
        public RangeFilter<DateTime> StartTimeRange { get; set; }
        public RangeFilter<DateTime> StopTimeRange { get; set; }
    }

    public class OperationPagingRequest : ServerSidePagingRequest<OperationPagingFilter>
    {
    }

    public class OperationPagingResponse : ServerSidePagingResponse<OperationWithLogs>
    {
    }
}