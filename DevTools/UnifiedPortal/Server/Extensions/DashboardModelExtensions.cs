using VXDesign.Store.DevTools.Common.Core.Entities.Dashboard;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Dashboard;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
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