using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.Incident;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Operation;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Incident
{
    public class IncidentBaseModel
    {
        public int AuthorId { get; set; }
        public string AuthorColor { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }

        public int? AssigneeId { get; set; }
        public string AssigneeColor { get; set; }
        public string AssigneeFirstName { get; set; }
        public string AssigneeLastName { get; set; }

        public string InitialTime { get; set; }
        public IncidentStatus Status { get; set; }
    }

    public class IncidentModel : IncidentBaseModel
    {
        public long OperationId { get; set; }
        public string Scope { get; set; }
        public string ContextName { get; set; }
        public bool? IsSuccessful { get; set; }
        public int? UserId { get; set; }
        public string Color { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }

        public IEnumerable<LogModel> Logs { get; set; }
        public IEnumerable<IncidentHistoryModel> History { get; set; }
    }

    public class IncidentUpdateModel
    {
        public int AuthorId { get; set; }
        public int AssigneeId { get; set; }
        public IncidentStatus Status { get; set; }
        public string Comment { get; set; }
    }

    public class IncidentCommentModel
    {
        public long? HistoryId { get; set; }
        public string Comment { get; set; }
    }
}