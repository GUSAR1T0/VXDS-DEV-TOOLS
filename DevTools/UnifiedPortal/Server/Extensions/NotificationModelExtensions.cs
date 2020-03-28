using VXDesign.Store.DevTools.Common.Core.Entities.Notification;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Notification;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class NotificationModelExtensions
    {
        internal static NotificationUpdateEntity ToEntity(this NotificationUpdateModel model) => new NotificationUpdateEntity
        {
            Id = model.Id,
            Message = model.Message,
            Level = model.Level,
            StartTime = model.StartTime.ToLocalTime(),
            StopTime = model.StopTime.ToLocalTime()
        };
    }
}