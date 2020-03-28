using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Notification
{
    public enum NotificationLevel : byte
    {
        [Description("Information")]
        Information = 1,

        [Description("Warning")]
        Warning = 2,

        [Description("Error")]
        Error = 3
    }
}