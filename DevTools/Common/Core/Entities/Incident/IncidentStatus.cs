using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Incident
{
    public enum IncidentStatus : byte
    {
        [Description("Opened")]
        Opened = 1,

        [Description("In Progress")]
        InProgress = 2,

        [Description("Resolved")]
        Resolved = 3,

        [Description("Cancelled")]
        Cancelled = 4
    }
}