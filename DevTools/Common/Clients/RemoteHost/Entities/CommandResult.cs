namespace VXDesign.Store.DevTools.Common.Clients.RemoteHost.Entities
{
    public class CommandResult
    {
        public string Command { get; set; }
        public string Output { get; set; }
        public string Error { get; set; }
        public int ExitStatus { get; set; }

        public bool HasErrors => !string.IsNullOrWhiteSpace(Error);
    }
}