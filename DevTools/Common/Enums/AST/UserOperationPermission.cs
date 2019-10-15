using System;
using System.ComponentModel;

namespace VXDesign.Store.DevTools.Common.Enums.AST
{
    [Flags]
    public enum UserOperationPermission
    {
        [Description("Edit list of user roles")]
        EditListOfUserRoles = 1,

        [Description("Edit operation permission list for user role")]
        EditOperationPermissionListForUserRole = 2,

        [Description("Manage user role")]
        ManageUserRole = 4
    }
}