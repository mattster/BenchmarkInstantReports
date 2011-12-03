using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.References
{
    public static class DatabaseDefn
    {
        public static string dbScans = "ACI.BENCHMARK";
        public static string dbTestDefn = "ACI.TEST_DEFINITION";
        public static string dbSchool = "ACI.SCHOOL";
        public static string dbAnswerKey = "ACI.ANSWER_KEY";
        public static string dbAnswerKeyCampus = "ACI.ANSWER_KEY_CAMPUS";
        public static string dbObjectives = "ACI.OBJECTIVES";

        public static string dbStudentRoster_reg = "SIS_ODS.RISD_STUDENT_ROSTER_VW";
        public static string dbStudentRoster_temp = "ACI.ROSTER_TEMP";

        public static string dbResultsBenchmarkStats = "ACI.TEMP_RESULTS_BENCHMARKSTATS";
        public static string dbResultsScanReport = "ACI.TEMP_RESULTS_SCANREPORT";
        public static string dbResultsStudentStats = "ACI.TEMP_RESULTS_STUDENTSTATS";

    }
}