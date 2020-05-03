using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Module
{
    public enum ModuleStatus
    {
        [Description("Created")]
        Created = 1,

        [Description("Installing")]
        Installing = 2,

        [Description("Installed")]
        Installed = 3,

        [Description("Updated To Upgrade")]
        UpdatedToUpgrade = 4,

        [Description("Upgrading")]
        Upgrading = 5,

        [Description("Upgraded")]
        Upgraded = 6,

        [Description("Updated To Downgrade")]
        UpdatedToDowngrade = 7,

        [Description("Downgrading")]
        Downgrading = 8,

        [Description("Downgraded")]
        Downgraded = 9,

        [Description("Updated To Uninstall")]
        UpdatedToUninstall = 10,

        [Description("Uninstalling")]
        Uninstalling = 11,

        [Description("Uninstalled")]
        Uninstalled = 12,

        [Description("Updated To Run")]
        UpdatedToRun = 13,

        [Description("Running")]
        Running = 14,

        [Description("Run")]
        Run = 15,

        [Description("Updated To Stop")]
        UpdatedToStop = 16,

        [Description("Stopping")]
        Stopping = 17,

        [Description("Stopped")]
        Stopped = 18
    }
}