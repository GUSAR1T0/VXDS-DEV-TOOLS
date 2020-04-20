using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Notification
{
    public enum NotificationLevel : byte
    {
        [Description("Information")]
        Information = 1,

        [Description("Success")]
        Success = 2,

        [Description("Warning")]
        Warning = 3,

        [Description("Error")]
        Error = 4
    }
}