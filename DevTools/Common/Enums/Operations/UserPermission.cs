using System;
using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Enums.Operations
{
    [Flags]
    public enum UserPermission
    {
        [Description("Access to Admin Panel")]
        AccessToAdminPanel = 1,

        [Description("Update User Profile")]
        UpdateUserProfile = 2,

        [Description("Manage User Roles")]
        ManageUserRoles = 4
    }
}