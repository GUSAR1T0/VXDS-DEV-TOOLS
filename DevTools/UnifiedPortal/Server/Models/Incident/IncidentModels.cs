using System;
using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.Incident;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Incident
{
    public class IncidentGetModel
    {
        public long OperationId { get; set; }
        public string Scope { get; set; }
        public string ContextName { get; set; }

        public int AuthorId { get; set; }
        public string AuthorColor { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }

        public int? AssigneeId { get; set; }
        public string AssigneeColor { get; set; }
        public string AssigneeFirstName { get; set; }
        public string AssigneeLastName { get; set; }

        public DateTime InitialTime { get; set; }
        public IncidentStatus Status { get; set; }
        public IEnumerable<IncidentHistoryModel> History { get; set; }
    }

    public class IncidentUpdateModel
    {
        public int AuthorId { get; set; }
        public int? AssigneeId { get; set; }
        public IncidentStatus Status { get; set; }
        public string Comment { get; set; }
    }
}