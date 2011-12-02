using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Exceptions
{
    public class AlternateAnswer : ExceptionItem
    {
        public string AltAnswer { get; set; }

        public AlternateAnswer(string testid, object campuses, object teachers, object periods, int item, string altanswer)
            : base(testid, campuses, teachers, periods, item)
        {
            AltAnswer = altanswer;
        }

        public AlternateAnswer(string testid, object campuses, object teachers, int item, string altanswer)
            : base(testid, campuses, teachers, item)
        {
            AltAnswer = altanswer;
        }

        public AlternateAnswer(string testid, object campuses, int item, string altanswer)
            : base(testid, campuses, item)
        {
            AltAnswer = altanswer;
        }

        public AlternateAnswer(string testid, int item, string altanswer)
            : base(testid, item)
        {
            AltAnswer = altanswer;
        }
    }
}