namespace VXDesign.Store.DevTools.Common.Clients.RemoteHost.Entities
{
    public abstract class BaseSshConnectionCredentials
    {
        public string Host { get; set; }
        public int? Port { get; set; }
    }

    public class SshConnectionByPasswordCredentials : BaseSshConnectionCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}