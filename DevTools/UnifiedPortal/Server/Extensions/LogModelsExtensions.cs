using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Storage.LogStorage;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Operation;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
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