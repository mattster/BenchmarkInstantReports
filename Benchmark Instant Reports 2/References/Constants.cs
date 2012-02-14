using System;

namespace Benchmark_Instant_Reports_2.References
{
    public static class Constants
    {
        #region DropDownLists
        // for use in DropDown Lists to indicate All Items
        public const string AllIndicator = "--ALL--";

        // used in drop down lists to separate groups
        public static string DropDownSeparatorString = "----";
        #endregion


        #region DatabaseQueryInfo
        // special database field concatenations used in queries
        public static string TeacherNameNumFieldName = "R.TEACHER_NAME||\'-\'||R.TEACHER_NBR";
        public static string TeacherNameFieldNameR = "R.TEACHER_NAME";
        public static string TeacherNameFieldName = "TEACHER_NAME";
        public static string TeacherNameNumFieldNameNew = "TEACHER_NAMENUM";

        // for substitution when building some queries
        public static string SchoolCriteriaInCustomQ1 = "AND R.SCHOOL2 = @school"; // "AND R.SCHOOL_ABBR = @school";
        public static string SchoolCriteriaInCustomQ2 = "AND SCHOOL2 = @school"; //"AND SCHOOL_ABBR = @school";
        public static string SchoolCriteriaInCustomQ3 = "and school2 = @school"; //"and school_abbr = @school";
        public static string SchoolCriteriaInCustomQ4 = "AND R.SCHOOL_ABBR = @school";
        public static string SchoolCriteriaInCustomQ5 = "AND SCHOOL_ABBR = @school";
        public static string SchoolCriteriaInCustomQ6 = "and school_abbr = @school";
        #endregion


        #region AnswerKeyInfo
        // maximum number of test questions
        public const int MaxNumTestQuestions = 200;

        // default amount to increment for additional answer keys
        public const int DfltAnsKeyIncrement = MaxNumTestQuestions;

        // indicates placeholders in the answer key
        public static int AnswerKeyFillerObjective = 99;
        public static string AnswerKeyFillerTEKS = "ZZZZZ";
        public static string ExcludeItemTEKSInd = "XXXXX";
        #endregion


        #region CourseInfo
        // elementary grade and course info
        public static string ElemGradeListForQuery = "\'01\',\'02\',\'03\',\'04\',\'05\',\'06\'";
        public static string ElemAttCourseListForQuery = "\'E9001\',\'E9002\',\'E9003\',\'E9004\',\'E9005\',\'E9006\'";
        #endregion


        #region ReportFormatting
        // for Student Summary report formatting
        public static int MaxFormattedAnsGroups = 7;
        public static int NumColumnsInFormattedLine = 75;
        public static string CorrectAnswerIndicator = "*";
        #endregion


        #region Cookies
        public static string SavedSelectedTestIDCookieName = "selectedTestID";
        public static string SavedSelectedTestIDsCokieName = "selectedTestIDs";
        public static string SavedSelectedCampusCookieName = "selectedCampus";
        public static int CookieDurationDays = 5;
        #endregion


        #region SchoolInfo
        public enum SchoolType
        {
            All,
            Elementary,
            JuniorHigh,
            HighSchool,
            AllSecondary,
            Unknown
        }
        public static string DispAllSecondary = "ALL Secondary";
        public static string DispAllElementary = "ALL Elementary";
        #endregion


        #region Misc
        public static string UsernameAllCampuses = "ALL";
        public static bool LookupStudentNamesFromRoster = true;
        public static string UnknownTeacherName = "zUNKNOWN TEACHER";
        public static string UnknownPeriod = "00";
        public static string UnknownCourseID = "0000";
        public static DateTime FirstDaySchoolYear = DateTime.Parse("08/22/2012");
        public static string TestSubjectBallot = "BALLOT";
        public static string TestSubjectSample = "SAMPLE";
#endregion


        #region TestIDs
        private static string TestID_Yr_RegEx1 = @"(\d{4})";
        private static string TestID_Month_RegEx2 = @"(\d{2})";
        private static string TestID_ElemSec_RegEx3 = @"([ES])";
        private static string TestID_Subj_RegEx4 = @"([A-Z]+)";
        private static string TestID_Name_RegEx5 = @"(.*)";
        private static string TestID_Window_RegEx6 = @"(\d{1,2})";
        private static string TestID_Key_RegEx7 = @"(\d{2})";

        public static string TestIDRegEx =
            @"^" + TestID_Yr_RegEx1 + "-" + TestID_Month_RegEx2 + 
            @"\s" + TestID_ElemSec_RegEx3 + TestID_Subj_RegEx4 +
            @"\s" + TestID_Name_RegEx5 + 
            @"\s" + TestID_Window_RegEx6 + "-" + TestID_Key_RegEx7 + 
            @"$";
        #endregion
    }
}