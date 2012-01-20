using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.References
{
    public static class Constants
    {
        // for use in DropDown Lists to indicate All Items
        public const string AllIndicator = "--ALL--";

        // maximum number of test questions
        public const int MaxNumTestQuestions = 200;

        // default amount to increment for additional answer keys
        public const int DfltAnsKeyIncrement = MaxNumTestQuestions;

        // used in drop down lists to separate groups
        public static string DropDownSeparatorString = "----";
        
        // special database field concatenations used in queries
        public static string TeacherNameNumFieldName = "R.TEACHER_NAME||\'-\'||R.TEACHER_NBR";
        public static string TeacherNameFieldNameR = "R.TEACHER_NAME";
        public static string TeacherNameFieldName = "TEACHER_NAME";
        public static string TeacherNameNumFieldNameNew = "TEACHER_NAMENUM";
        
        // elementary grade and course info
        public static string ElemGradeListForQuery = "\'01\',\'02\',\'03\',\'04\',\'05\',\'06\'";
        public static string ElemAttCourseListForQuery = "\'E9001\',\'E9002\',\'E9003\',\'E9004\',\'E9005\',\'E9006\'";
        
        // indicates placeholders in the answer key
        public static int AnswerKeyFillerObjective = 99;
        public static string AnswerKeyFillerTEKS = "ZZZZZ";
        public static string ExcludeItemTEKSInd = "XXXXX";
        
        // for substitution when building some queries
        public static string SchoolCriteriaInCustomQ1 = "AND R.SCHOOL2 = @school"; // "AND R.SCHOOL_ABBR = @school";
        public static string SchoolCriteriaInCustomQ2 = "AND SCHOOL2 = @school"; //"AND SCHOOL_ABBR = @school";
        public static string SchoolCriteriaInCustomQ3 = "and school2 = @school"; //"and school_abbr = @school";
        public static string SchoolCriteriaInCustomQ4 = "AND R.SCHOOL_ABBR = @school";
        public static string SchoolCriteriaInCustomQ5 = "AND SCHOOL_ABBR = @school";
        public static string SchoolCriteriaInCustomQ6 = "and school_abbr = @school";

        // for Student Summary report formatting
        public static int maxFormattedAnsGroups = 7;
        public static int numColumnsInFormattedLine = 75;



        public static string UsernameAllCampuses = "ALL";
        public static bool LookupStudentNamesFromRoster = true;
        public static string UnknownTeacherName = "zUNKNOWN TEACHER";
    }
}