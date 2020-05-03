using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Module
{
    public enum ModuleConfigurationVerdict
    {
        [Description("New module")]
        NewModule = 1,

        [Description("Installed")]
        Installed = 2,

        [Description("Upgrade")]
        Upgrade = 3,

        [Description("Downgrade")]
        Downgrade = 4,

        [Description("Module Is Being Updated")]
        Updating = 5,

        [Description("Broken Changes")]
        BrokenChanges = 6
    }
}