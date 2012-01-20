using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    /// <summary>
    /// used as a collection of Answer Key Items (AnswerKeyItem)
    /// </summary>
    public class AnswerKeyItemData : DataItemCollection<AnswerKeyItem>
    {
        public AnswerKeyItemData() : base() { }

        public AnswerKeyItemData(List<AnswerKeyItem> items) : base(items) { }
    }
}