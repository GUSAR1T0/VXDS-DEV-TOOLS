using VXDesign.Store.DevTools.Core.Entities.Storage.Dashboard;
using VXDesign.Store.DevTools.UnifiedPortal.Models.Dashboard;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class DashboardModelExtensions
    {
        internal static DashboardModel ToModel(this DashboardEntity entity) => new DashboardModel
        {
            UsersCount = entity.UsersCount,
            RolesCount = entity.RolesCount,
            OperationsCount = entity.OperationsCount,
            LogsCount = entity.LogsCount
        };
    }
}