using System;
using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.SSP;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Notification
{
    public class NotificationEntity : IDataEntity, IPagingResponseItemEntity
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public NotificationLevel Level { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
    }

    public class NotificationPagingFilter : IPagingFilterEntity
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<NotificationLevel> Levels { get; set; }
        public RangeFilter<DateTime> StartTimeRange { get; set; }
        public RangeFilter<DateTime> StopTimeRange { get; set; }
        public bool? IsActive { get; set; }
    }

    public class NotificationPagingRequest : ServerSidePagingRequest<NotificationPagingFilter>
    {
    }

    public class NotificationPagingResponse : ServerSidePagingResponse<NotificationEntity>
    {
    }

    public class NotificationUpdateEntity : IDataEntity
    {
        public int? Id { get; set; }
        public string Message { get; set; }
        public NotificationLevel Level { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
    }
}