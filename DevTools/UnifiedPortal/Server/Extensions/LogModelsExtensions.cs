using VXDesign.Store.DevTools.Core.Entities.Common;
using VXDesign.Store.DevTools.Core.Entities.Storage.Operation;
using VXDesign.Store.DevTools.Core.Extensions.Controllers;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Operation;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class LogModelsExtensions
    {
        internal static LogModel ToModel(this LogEntity entity) => new LogModel
        {
            Id = entity.Id,
            Level = entity.Level,
            DateTime = entity.DateTime.FormatDateTime(FormatPattern.FullDateTimeWithDayOfWeek),
            Logger = entity.Logger,
            Message = entity.Message,
            Value = entity.Value
        };
    }
}