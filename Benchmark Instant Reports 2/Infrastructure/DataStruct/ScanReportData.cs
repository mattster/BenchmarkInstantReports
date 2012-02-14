using System.Collections.Generic;

namespace Benchmark_Instant_Reports_2.Infrastructure.DataStruct
{
    public class ScanReportData : DataItemCollection<ScanReportItem>
    {
        public ScanReportData() : base() { }

        public ScanReportData(List<ScanReportItem> items) : base(items) { }
    }
}