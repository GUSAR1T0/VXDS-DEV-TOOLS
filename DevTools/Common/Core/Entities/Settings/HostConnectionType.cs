using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Settings
{
    public enum HostConnectionType : byte
    {
        [Description("Shell")]
        Shell = 1,

        [Description("SSH")]
        SSH = 2
    }
}