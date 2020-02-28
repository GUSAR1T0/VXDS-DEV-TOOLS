namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Operation
{
    public class LogModel
    {
        public string Id { get; set; }
        public string Level { get; set; }
        public string DateTime { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public object Value { get; set; }
    }
}