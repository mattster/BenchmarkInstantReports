using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Exceptions
{
    public class GridNonExactMatch : ExceptionItem
    {
        public GridNonExactMatch(string testid, object campuses, object teachers, object periods, int[] items)
            : base(testid, campuses, teachers, periods, items)
        {

        }

        public GridNonExactMatch(string testid, object campuses, object teachers, object periods, int item)
            : base(testid, campuses, teachers, periods, item)
        {

        }

        public GridNonExactMatch(string testid, object campuses, object teachers, int[] items)
            : base(testid, campuses, teachers, items)
        {

        }

        public GridNonExactMatch(string testid, object campuses, object teachers, int item)
            : base(testid, campuses, teachers, item)
        {

        }

        public GridNonExactMatch(string testid, object campuses, int[] items)
            : base(testid, campuses, items)
        {

        }

        public GridNonExactMatch(string testid, object campuses, int item)
            : base(testid, campuses, item)
        {

        }

        public GridNonExactMatch(string testid, int[] items)
            : base(testid, items)
        {

        }

        public GridNonExactMatch(string testid, int item)
            : base(testid, item)
        {

        }

    }
}