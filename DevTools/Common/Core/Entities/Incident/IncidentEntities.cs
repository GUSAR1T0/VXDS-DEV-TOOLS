using System;
using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Entities;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Incident
{
    public class IncidentBaseEntity
    {
        public long IncidentOperationId { get; set; }
        public int AuthorId { get; set; }
        public int? AssigneeId { get; set; }
        public IncidentStatus Status { get; set; }
    }

    public class IncidentUpdateEntity : IncidentBaseEntity
    {
        public int? ChangedBy { get; set; }
        public string Comment { get; set; }
    }

    public class IncidentEntity : IncidentBaseEntity
    {
        public string AuthorColor { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }

        public string AssigneeColor { get; set; }
        public string AssigneeFirstName { get; set; }
        public string AssigneeLastName { get; set; }

        public DateTime InitialTime { get; set; }
    }

    public class IncidentFullEntity : IncidentEntity
    {
        public string Scope { get; set; }
        public string ContextName { get; set; }
        public bool? IsSuccessful { get; set; }
        public int? UserId { get; set; }
        public string Color { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsSystemAction { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }

        public IEnumerable<LogEntity> Logs { get; set; }
        public IEnumerable<IncidentHistoryEntity> History { get; set; }
    }
}