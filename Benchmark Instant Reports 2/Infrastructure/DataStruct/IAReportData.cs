using System.Collections.Generic;

namespace Benchmark_Instant_Reports_2.Infrastructure.DataStruct
{
    /// <summary>
    /// used as a collection of Item Analysis Report items (IAReportItem)
    /// </summary>
    public class IAReportData : DataItemCollection<IAReportItem>
    {
        public IAReportData() : base() { }

        public IAReportData(List<IAReportItem> items) : base(items) { }

    }
}
