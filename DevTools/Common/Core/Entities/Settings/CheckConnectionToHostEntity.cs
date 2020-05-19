namespace VXDesign.Store.DevTools.Common.Core.Entities.Settings
{
    public class CheckConnectionToHostEntity
    {
        public HostOperatingSystem OperatingSystem { get; set; }
        public string Host { get; set; }
        public HostConnectionType Type { get; set; }
        public int? Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}