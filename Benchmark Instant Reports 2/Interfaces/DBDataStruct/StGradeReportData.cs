using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
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