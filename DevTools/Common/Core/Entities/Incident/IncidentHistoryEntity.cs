using System;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Incident
{
    public class IncidentHistoryRecordEntity
    {
        public int? AuthorId { get; set; }
        public int? AssigneeId { get; set; }
        public bool? IsUnassigned { get; set; }
        public IncidentStatus? Status { get; set; }
    }

    public class IncidentHistoryEntity : IncidentHistoryRecordEntity
    {
        public long OperationId { get; set; }

        public string AuthorColor { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }

        public string AssigneeColor { get; set; }
        public string AssigneeFirstName { get; set; }
        public string AssigneeLastName { get; set; }

        public DateTime ChangeTime { get; set; }
        public string Comment { get; set; }
    }
}