using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Settings
{
    public enum HostOperationSystem : byte
    {
        [Description("Windows OS")]
        WindowsOS = 1,

        [Description("Linux")]
        Linux = 2,

        [Description("macOS")]
        MacOS = 3
    }
}