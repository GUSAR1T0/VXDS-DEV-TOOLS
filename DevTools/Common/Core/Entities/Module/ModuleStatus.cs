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

        [Description("Failed To Install")]
        FailedToInstall = 4,

        [Description("Updated To Upgrade")]
        UpdatedToUpgrade = 5,

        [Description("Upgrading")]
        Upgrading = 6,

        [Description("Upgraded")]
        Upgraded = 7,

        [Description("Failed To Upgrade")]
        FailedToUpgrade = 8,

        [Description("Updated To Downgrade")]
        UpdatedToDowngrade = 9,

        [Description("Downgrading")]
        Downgrading = 10,

        [Description("Downgraded")]
        Downgraded = 11,

        [Description("Failed To Downgrade")]
        FailedToDowngrade = 12,

        [Description("Updated To Uninstall")]
        UpdatedToUninstall = 13,

        [Description("Uninstalling")]
        Uninstalling = 14,

        [Description("Uninstalled")]
        Uninstalled = 15,

        [Description("Failed To Uninstall")]
        FailedToUninstall = 16,

        [Description("Updated To Run")]
        UpdatedToRun = 17,

        [Description("Running")]
        Running = 18,

        [Description("Run")]
        Run = 19,

        [Description("Failed To Run")]
        FailedToRun = 20,

        [Description("Updated To Stop")]
        UpdatedToStop = 21,

        [Description("Stopping")]
        Stopping = 22,

        [Description("Stopped")]
        Stopped = 23,

        [Description("Failed To Stopped")]
        FailedToStop = 24
    }
}