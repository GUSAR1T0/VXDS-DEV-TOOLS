using System;
using System.ComponentModel;

namespace VXDesign.Store.DevTools.Core.Enums.Operations
{
    [Flags]
    public enum PortalPermission
    {
        [Description("Access to Admin Panel")]
        AccessToAdminPanel = 1,

        [Description("Manage User Profiles")]
        ManageUserProfiles = 2,

        [Description("Manage User Roles")]
        ManageUserRoles = 4,

        [Description("Manage Settings")]
        ManageSettings = 8,

        [Description("Manage Projects")]
        ManageProjects = 16
    }
}