using System;
using VXDesign.Store.DevTools.Common.Core.Entities.Incident;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Incident
{
    public class IncidentHistoryModel
    {
        public long OperationId { get; set; }

        public int? AuthorId { get; set; }
        public string AuthorColor { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }

        public int? AssigneeId { get; set; }
        public bool? IsUnassigned { get; set; }
        public string AssigneeColor { get; set; }
        public string AssigneeFirstName { get; set; }
        public string AssigneeLastName { get; set; }

        public IncidentStatus? Status { get; set; }
        public DateTime ChangeTime { get; set; }
        public string Comment { get; set; }
    }
}