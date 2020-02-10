using VXDesign.Store.DevTools.Core.Entities.Storage.Log;
using VXDesign.Store.DevTools.UnifiedPortal.Models.Log;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class LogModelsExtensions
    {
        internal static LogModel ToModel(this LogEntity entity) => new LogModel
        {
            Id = entity.Id,
            Level = entity.Level,
            DateTime = entity.DateTime,
            Logger = entity.Logger,
            Message = entity.Message,
            Value = entity.Value
        };
    }
}