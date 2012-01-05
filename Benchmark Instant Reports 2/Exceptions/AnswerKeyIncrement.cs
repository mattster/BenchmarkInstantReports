using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Exceptions
{
    public class AnswerKeyIncrement : ExceptionItem
    {
        public int IncrementAmt { 
            get { return Items[0]; } 
            set { Items = new int[] { value }; }
        }


        public AnswerKeyIncrement(string testID, object campuses, object teachers, object periods, int items = Constants.DfltAnsKeyIncrement)
            : base(testID, campuses, teachers, periods, items)
        {

        }

        public AnswerKeyIncrement(string testID, object campuses, object teachers, int items = Constants.DfltAnsKeyIncrement)
            : base(testID, campuses, teachers, items)
        {

        }

        public AnswerKeyIncrement(string testID, object campuses, int items = Constants.DfltAnsKeyIncrement)
            : base(testID, campuses, items)
        {

        }

    }
}