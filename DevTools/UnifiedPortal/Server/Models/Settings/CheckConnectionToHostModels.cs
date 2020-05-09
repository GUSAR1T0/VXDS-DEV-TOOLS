using VXDesign.Store.DevTools.Common.Clients.RemoteHost.Entities;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Settings
{
    public class CheckConnectionToHostModel
    {
        public HostOperatingSystem OperatingSystem { get; set; }
        public string Host { get; set; }
        public HostConnectionType Type { get; set; }
        public int? Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class CheckConnectionsToHostResultModel
    {
        public HostCredentialsItemModel Item { get; set; }
        public CommandResult Result { get; set; }
    }
}