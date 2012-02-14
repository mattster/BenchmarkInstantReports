using System.Collections.Generic;

namespace Benchmark_Instant_Reports_2.Infrastructure.DataStruct
{
	public class DataToGradeItemCollection : DataItemCollection<DataToGradeItem>
	{
        public DataToGradeItemCollection() : base() { }

        public DataToGradeItemCollection(List<DataToGradeItem> items) : base(items) { }
	}
}