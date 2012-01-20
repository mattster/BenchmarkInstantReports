using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
	public class StudentListItemData : DataItemCollection<StudentListItem>
	{
        public StudentListItemData() : base() { }

        public StudentListItemData(List<StudentListItem> items) : base(items) { }
	}
}