using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Exceptions
{
    public class DropList : ExceptionItem
    {
        public DropList(string testid, object campuses, object teachers, object periods, int[] items)
            : base(testid, campuses, teachers, periods, items)
        {

        }

        public DropList(string testid, object campuses, object teachers, object periods, int item)
            : base(testid, campuses, teachers, periods, item)
        {

        }

        public DropList(string testid, object campuses, object teachers, int[] items)
            : base(testid, campuses, teachers, items)
        {

        }

        public DropList(string testid, object campuses, object teachers, int item)
            : base(testid, campuses, teachers, item)
        {

        }

        public DropList(string testid, object campuses, int[] items)
            : base(testid, campuses, items)
        {

        }

        public DropList(string testid, object campuses, int item)
            : base(testid, campuses, item)
        {

        }

        public DropList(string testid, int[] items)
            : base(testid, items)
        {

        }

        public DropList(string testid, int item)
            : base(testid, item)
        {

        }
    }
}