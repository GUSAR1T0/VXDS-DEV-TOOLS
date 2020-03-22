using System.Collections.Generic;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Dashboard
{
    public class SystemStatisticsDataModel
    {
        public IEnumerable<string> Dates { get; set; }
        public IEnumerable<long> Operations { get; set; }
        public long OperationsTotal { get; set; }
        public IEnumerable<long> Logs { get; set; }
        public long LogsTotal { get; set; }
    }
}