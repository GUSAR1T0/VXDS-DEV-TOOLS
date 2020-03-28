using System;
using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.Notification;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Common;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.SSP;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Notification
{
    public class NotificationModel : PagingResponseItemModel, IPagingResponseItemModel<NotificationModel, NotificationEntity>
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public NotificationLevel Level { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }

        public NotificationModel ToModel(NotificationEntity entity)
        {
            Id = entity.Id;
            Message = entity.Message;
            Level = entity.Level;
            StartTime = entity.StartTime.FormatDateTime(FormatPattern.FullDateTimeWithDayOfWeek);
            StopTime = entity.StopTime.FormatDateTime(FormatPattern.FullDateTimeWithDayOfWeek);
            return this;
        }
    }

    public class NotificationPagingFilterModel : PagingFilterModel, IPagingFilterModel<NotificationPagingFilter>
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<NotificationLevel> Levels { get; set; }
        public RangeFilterModel<DateTime> StartTimeRange { get; set; }
        public RangeFilterModel<DateTime> StopTimeRange { get; set; }
        public bool? IsActive { get; set; }

        public NotificationPagingFilter ToEntity() => new NotificationPagingFilter
        {
            Ids = Ids,
            Levels = Levels,
            StartTimeRange = StartTimeRange.ToEntity(),
            StopTimeRange = StopTimeRange.ToEntity(),
            IsActive = IsActive
        };
    }

    public class NotificationPagingRequestModel : ServerSidePagingRequestModel<NotificationPagingFilterModel, NotificationPagingRequest>
    {
        public override NotificationPagingRequest ToEntity() => new NotificationPagingRequest
        {
            PageNo = PageNo,
            PageSize = PageSize,
            Filter = Filter.ToEntity()
        };
    }

    public class NotificationPagingResponseModel : ServerSidePagingResponseModel<NotificationModel, NotificationPagingResponseModel, NotificationPagingResponse>
    {
        public override NotificationPagingResponseModel ToModel(NotificationPagingResponse entity)
        {
            Total = entity.Total;
            Items = entity.Items.Select(item => new NotificationModel().ToModel(item));
            return this;
        }
    }
}