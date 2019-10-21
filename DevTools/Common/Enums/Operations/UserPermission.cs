using System;
using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Enums.Operations
{
    [Flags]
    public enum UserPermission
    {
        [Description("Update user profile")]
        UpdateUserProfile = 1,

        [Description("Delete user")]
        DeleteUser = 2
    }
}