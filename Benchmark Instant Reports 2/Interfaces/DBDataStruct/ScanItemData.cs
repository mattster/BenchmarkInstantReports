using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    /// <summary>
    /// used as a collection of Scan Data items (ScanItem)
    /// </summary>
    public class ScanItemData : DataItemCollection<ScanItem>
    {
        public ScanItemData() : base() { }

        public ScanItemData(List<ScanItem> items) : base(items) { }
    }
}