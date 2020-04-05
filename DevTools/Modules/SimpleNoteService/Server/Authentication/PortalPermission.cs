using System;
using System.ComponentModel;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Authentication
{
    [Flags]
    public enum PortalPermission
    {
        [Description("Access to Module")]
        AccessToModule = 1
    }
}