
namespace Benchmark_Instant_Reports_2.References
{
    public static class DatabaseDefn
    {
        public static string DBScans = "ACI.BENCHMARK";
        public static string DBTestDefn = "ACI.TEST_DEFINITION";
        public static string DBSchool = "ACI.SCHOOL";
        public static string DBAnswerKey = "ACI.ANSWER_KEY";
        public static string DBAnswerKeyCampus = "ACI.ANSWER_KEY_CAMPUS";
        public static string DBObjectives = "ACI.OBJECTIVES";

        public static string DBStudentRoster = "SIS_ODS.RISD_STUDENT_ROSTER_VW";
        public static string DBStudentRoster_temp = "ACI.ROSTER_TEMP";

        public static string DBResultsBenchmarkStats = "ACI.TEMP_RESULTS_BENCHMARKSTATS";
        public static string DBResultsScanReport = "ACI.TEMP_RESULTS_SCANREPORT";
        public static string DBResultsStudentStats = "ACI.TEMP_RESULTS_STUDENTSTATS";

        // the database name used in the connection string
        public static string databaseName = "SISPRODOracleDb";

    }
}