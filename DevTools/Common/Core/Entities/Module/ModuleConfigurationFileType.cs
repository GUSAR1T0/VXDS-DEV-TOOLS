using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Module
{
    public enum ModuleConfigurationFileType
    {
        [Description("YAML")]
        YAML = 1,

        [Description("JSON")]
        JSON = 2
    }
}