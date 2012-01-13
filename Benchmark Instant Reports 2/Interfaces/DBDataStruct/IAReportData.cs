using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    /// <summary>
    /// used as a collection of Item Analysis Report items (IAReportItem)
    /// </summary>
    public class IAReportData : DataItemCollection<IAReportItem>
    {
        public IAReportData() : base() { }

        public IAReportData(List<IAReportItem> items) : base(items) { }

        //public IEnumerable<IAReportItem> GetItems();
    }
}
