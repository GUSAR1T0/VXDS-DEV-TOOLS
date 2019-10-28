using VXDesign.Store.DevTools.Common.Entities.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Models.AdminPanel;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class DashboardModelExtensions
    {
        internal static DashboardModel ToModel(this DashboardEntity entity) => new DashboardModel
        {
            RolesCount = entity.RolesCount,
            LogsCount = entity.LogsCount
        };
    }
}