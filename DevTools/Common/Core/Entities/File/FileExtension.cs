using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Core.Entities.File
{
    public enum FileExtension
    {
        [Description("Undefined")]
        Undefined = 0,

        [Description("YAML/YML")]
        YAML = 1,

        [Description("JSON")]
        JSON = 2
    }
}