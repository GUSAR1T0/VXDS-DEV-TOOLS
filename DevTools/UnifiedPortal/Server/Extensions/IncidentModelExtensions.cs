using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.Incident;
using VXDesign.Store.DevTools.Common.Core.Entities.Operation;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Incident;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class IncidentModelExtensions
    {
        internal static IncidentBaseModel ToIncidentModel(this OperationEntity entity) => entity.HasIncident
            ? new IncidentBaseModel
            {
                AuthorId = entity.IncidentAuthorId,
                AuthorColor = entity.IncidentAuthorColor,
                AuthorFirstName = entity.IncidentAuthorFirstName,
                AuthorLastName = entity.IncidentAuthorLastName,
                AssigneeId = entity.IncidentAssigneeId,
                AssigneeColor = entity.IncidentAssigneeColor,
                AssigneeFirstName = entity.IncidentAssigneeFirstName,
                AssigneeLastName = entity.IncidentAssigneeLastName,
                InitialTime = entity.IncidentInitialTime.FormatDateTime(FormatPattern.FullDateTimeWithDayOfWeek),
                Status = entity.IncidentStatus
            }
            : null;

        internal static IncidentHistoryModel ToModel(this IncidentHistoryEntity entity) => new IncidentHistoryModel
        {
            Id = entity.Id,
            OperationId = entity.OperationId,
            AuthorId = entity.AuthorId,
            AuthorColor = entity.AuthorColor,
            AuthorFirstName = entity.AuthorFirstName,
            AuthorLastName = entity.AuthorLastName,
            AssigneeId = entity.AssigneeId,
            IsUnassigned = entity.IsUnassigned,
            AssigneeColor = entity.AssigneeColor,
            AssigneeFirstName = entity.AssigneeFirstName,
            AssigneeLastName = entity.AssigneeLastName,
            Status = entity.Status,
            ChangedBy = entity.ChangedBy,
            ChangedByColor = entity.ChangedByColor,
            ChangedByFirstName = entity.ChangedBy.HasValue ? entity.ChangedByFirstName : "System",
            ChangedByLastName = entity.ChangedByLastName,
            ChangeTime = entity.ChangeTime.FormatDateTime(FormatPattern.FullDateTimeWithDayOfWeek),
            Comment = entity.Comment
        };

        internal static IncidentModel ToModel(this IncidentFullEntity entity) => new IncidentModel
        {
            OperationId = entity.IncidentOperationId,
            Scope = entity.Scope,
            ContextName = entity.ContextName,
            IsSuccessful = entity.IsSuccessful,
            UserId = entity.UserId,
            Color = entity.Color,
            FirstName = !entity.IsSystemAction ? entity.FirstName : "System",
            LastName = entity.LastName,
            StartTime = entity.StartTime.FormatDateTime(FormatPattern.FullDateTimeWithDayOfWeek),
            StopTime = entity.StopTime.FormatDateTime(FormatPattern.FullDateTimeWithDayOfWeek),
            AuthorId = entity.AuthorId,
            AuthorColor = entity.AuthorColor,
            AuthorFirstName = entity.AuthorFirstName,
            AuthorLastName = entity.AuthorLastName,
            AssigneeId = entity.AssigneeId,
            AssigneeColor = entity.AssigneeColor,
            AssigneeFirstName = entity.AssigneeFirstName,
            AssigneeLastName = entity.AssigneeLastName,
            InitialTime = entity.InitialTime.FormatDateTime(FormatPattern.FullDateTimeWithDayOfWeek),
            Status = entity.Status,
            Logs = entity.Logs.Select(item => item.ToModel()),
            History = entity.History.Select(item => item.ToModel())
        };

        internal static IncidentUpdateEntity ToEntity(this IncidentUpdateModel model, int? userId) => new IncidentUpdateEntity
        {
            AuthorId = model.AuthorId,
            AssigneeId = model.AssigneeId > 0 ? model.AssigneeId : (int?) null,
            Status = model.Status,
            ChangedBy = userId,
            Comment = !string.IsNullOrWhiteSpace(model.Comment) ? model.Comment : null
        };
    }
}