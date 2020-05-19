using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.HealthCheck;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.HealthCheck;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class HealthChecksModelExtensions
    {
        internal static IEnumerable<HealthCheckModel> ToModel(this IEnumerable<HealthCheckEntity> entities) => entities.Select(entity => new HealthCheckModel
        {
            Type = entity.Type,
            IsOk = entity.IsOk
        });
    }
}