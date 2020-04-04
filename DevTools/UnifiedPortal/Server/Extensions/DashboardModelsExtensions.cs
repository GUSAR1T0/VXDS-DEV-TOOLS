using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.Dashboard;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Dashboard;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class DashboardModelsExtensions
    {
        internal static NotificationsDataModel ToModel(this NotificationsDataEntity entity) => new NotificationsDataModel
        {
            NotificationsCount = entity.NotificationsCount
        };

        internal static IncidentsDataModel ToModel(this IncidentsDataEntity entity) => new IncidentsDataModel
        {
            AssigneeIncidentsCount = entity.AssigneeIncidentsCount,
            AuthorIncidentsCount = entity.AuthorIncidentsCount
        };

        internal static UsersDataModel ToModel(this UsersDataEntity entity) => new UsersDataModel
        {
            ActivatedCount = entity.ActivatedCount,
            DeactivatedCount = entity.DeactivatedCount
        };

        internal static UserRolesDataModel ToModel(this UserRolesDataEntity entity) => new UserRolesDataModel
        {
            UserRoles = entity.UserRoles.Select(item => new UserRoleDataModel
            {
                Name = item.Name,
                Count = item.Count
            }),
            Total = entity.Total
        };

        internal static ProjectsDataModel ToModel(this ProjectsDataEntity entity) => new ProjectsDataModel
        {
            ActiveCount = entity.ActiveCount,
            InactiveCount = entity.InactiveCount
        };

        internal static SystemStatisticsDataModel ToModel(this SystemStatisticsDataEntity entity)
        {
            var dates = new List<string>();
            var operations = new List<long>();
            var logs = new List<long>();

            for (var date = entity.SevenDaysAgo; date <= entity.Today; date = date.AddDays(1))
            {
                dates.Add(date.ToString("dd/MM/yyyy"));
                operations.Add(entity.Operations.FirstOrDefault(item => item.Date == date)?.Count ?? 0L);
                logs.Add(entity.Logs.FirstOrDefault(item => item.Date == date)?.Count ?? 0L);
            }

            return new SystemStatisticsDataModel
            {
                Dates = dates,
                Operations = operations,
                OperationsTotal = entity.OperationsTotal,
                Logs = logs,
                LogsTotal = entity.LogsTotal
            };
        }
    }
}