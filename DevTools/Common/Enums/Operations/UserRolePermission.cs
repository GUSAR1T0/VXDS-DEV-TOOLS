using System;
using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Enums.Operations
{
    [Flags]
    public enum UserRolePermission : byte
    {
        [Description("Create Role")]
        CreateRole = 1,

        [Description("Update Role")]
        UpdateRole = 2,

        [Description("Delete Role")]
        DeleteRole = 4
    }
}