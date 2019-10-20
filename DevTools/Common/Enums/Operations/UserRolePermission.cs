using System;
using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Enums.Operations
{
    [Flags]
    public enum UserRolePermission : byte
    {
        [Description("Create role")]
        CreateRole = 1,

        [Description("Update role")]
        UpdateRole = 2,

        [Description("Delete role")]
        DeleteRole = 4
    }
}