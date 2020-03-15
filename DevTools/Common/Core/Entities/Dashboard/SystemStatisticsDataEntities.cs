using System;
using System.Collections.Generic;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Dashboard
{
    public class SystemStatisticsDataByDateEntity : IDataEntity
    {
        public DateTime Date { get; set; }
        public long Count { get; set; }
    }

    public class SystemStatisticsDataEntity
    {
        public DateTime SevenDaysAgo { get; set; }
        public DateTime Today { get; set; }
        public IEnumerable<SystemStatisticsDataByDateEntity> Operations { get; set; }
        public long OperationsTotal { get; set; }
        public IEnumerable<SystemStatisticsDataByDateEntity> Logs { get; set; }
        public long LogsTotal { get; set; }
    }
}