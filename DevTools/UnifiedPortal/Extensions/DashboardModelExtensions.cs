using VXDesign.Store.DevTools.Common.Entities.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Models.Dashboard;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class DashboardModelExtensions
    {
        internal static DashboardModel ToModel(this DashboardEntity entity) => new DashboardModel
        {
            UsersCount = entity.UsersCount,
            RolesCount = entity.RolesCount,
            LogsCount = entity.LogsCount
        };
    }
}