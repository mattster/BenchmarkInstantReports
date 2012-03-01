using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Exceptions
{
    public static partial class ExceptionData
    {
        public static List<AlternateAnswer> alternateAnswers = new List<AlternateAnswer> {
            new AlternateAnswer("2011-09 SC Physics Unit 1 T2 CUT 91-21", 61, "-980"),

            new AlternateAnswer("2011-09 SC Biology Unit 2 CUT 99-21", "RHS", 3, "B"),
            new AlternateAnswer("2011-09 SC Biology Unit 2 PreAP CUT 99-23", "RHS", 3, "B"),

            new AlternateAnswer("2011-10 SS US History Unit 4 CUT 81-41", 13, "C"),

            new AlternateAnswer("2011-12 SC Chemistry Unit 6 T2-T4 CUT 90-61", "LHHS", 61, "149.1"),

            new AlternateAnswer("2012-01 EM Grade 5 Math-M SIM 21-68", 9, "C"),

            new AlternateAnswer("2012-03 SC Physics Unit 4 T4 CUT 91-81", 61, "6.0"),

            new AlternateAnswer("2012-02 SM Exit Level Math SIM 30-99", 49, "B"),
        };

    }
}