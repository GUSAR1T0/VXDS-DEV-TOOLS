using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Core.HTTP
{
    public enum HttpMethod
    {
        [Description("GET")]
        Get,

        [Description("POST")]
        Post,

        [Description("POST")]
        PostFile,

        [Description("PUT")]
        Put,

        [Description("DELETE")]
        Delete,

        [Description("OPTIONS")]
        Options
    }
}