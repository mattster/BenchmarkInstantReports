using System.Collections.Generic;

namespace Benchmark_Instant_Reports_2.Infrastructure.DataStruct
{
    /// <summary>
    /// used as a collection of Student Grade Report items (StGradeReportItem)
    /// </summary>
    public class StGradeReportData : DataItemCollection<StGradeReportItem>
    {
        public StGradeReportData() : base() { }

        public StGradeReportData(List<StGradeReportItem> items) : base(items) { }

    }
}