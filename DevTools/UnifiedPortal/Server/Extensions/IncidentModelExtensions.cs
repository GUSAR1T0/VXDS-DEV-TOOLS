using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.Incident;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Incident;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class IncidentModelExtensions
    {
        internal static IncidentHistoryModel ToModel(this IncidentHistoryEntity entity) => new IncidentHistoryModel
        {
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
            ChangeTime = entity.ChangeTime,
            Comment = entity.Comment
        };

        internal static IncidentGetModel ToModel(this IncidentFullEntity entity) => new IncidentGetModel
        {
            OperationId = entity.OperationId,
            Scope = entity.Scope,
            ContextName = entity.ContextName,
            AuthorId = entity.AuthorId,
            AuthorColor = entity.AuthorColor,
            AuthorFirstName = entity.AuthorFirstName,
            AuthorLastName = entity.AuthorLastName,
            AssigneeId = entity.AssigneeId,
            AssigneeColor = entity.AssigneeColor,
            AssigneeFirstName = entity.AssigneeFirstName,
            AssigneeLastName = entity.AssigneeLastName,
            InitialTime = entity.InitialTime,
            Status = entity.Status,
            History = entity.History.Select(item => item.ToModel())
        };

        internal static IncidentUpdateEntity ToEntity(this IncidentUpdateModel model) => new IncidentUpdateEntity
        {
            AuthorId = model.AuthorId,
            AssigneeId = model.AssigneeId,
            Status = model.Status,
            Comment = model.Comment
        };
    }
}