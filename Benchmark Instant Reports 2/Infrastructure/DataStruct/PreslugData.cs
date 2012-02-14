using System.Collections.Generic;

namespace Benchmark_Instant_Reports_2.Infrastructure.DataStruct
{
    public class PreslugData : DataItemCollection<PreslugItem>
    {
        public PreslugData() : base() { }

        public PreslugData(List<PreslugItem> items) : base(items) { }
    }
}