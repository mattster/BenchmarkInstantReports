
namespace Benchmark_Instant_Reports_2.References
{
    public static class Queries
    {

        // ***** get campuses
        #region get_campuses

        private static string GetSeparator =
        "select cast('" + Constants.DropDownSeparatorString + "' as nvarchar2(4)) as school_abbr, " +
        "cast('" + Constants.DropDownSeparatorString + "' as varchar2(50)) as schoolname from dual";

        public static string GetHSCampuses =
                "select schoolid, schoolname, schoolpassword, principal, area, loc, phone, " +
                " username, school_abbr, clusternum " +
                "from " + DatabaseDefn.DBSchool + " " +
                "where schoolid between 1 and 9 or schoolid = 52";

        public static string GetJHCampuses =
                "select schoolid, schoolname, schoolpassword, principal, area, loc, phone, " +
                " username, school_abbr, clusternum " +
                "from " + DatabaseDefn.DBSchool + " " +
                "where (schoolid between 10 and 99 and schoolid != 52) or schoolid = 6";

        public static string GetSECCampuses =
                "select schoolid, schoolname, schoolpassword, principal, area, loc, phone, " +
                " username, school_abbr, clusternum " +
                "from " + DatabaseDefn.DBSchool + " " +
                "where schoolid < 100";

        public static string GetELCampuses =
                "select school_abbr, schoolname from " + DatabaseDefn.DBSchool + " " +
                "where schoolid between 100 and 999 or schoolid = 6 order by schoolname";

        public static string GetSchoolByID =
                "select schoolid, schoolname, schoolpassword, principal, area, loc, phone, " +
                " username, school_abbr, clusternum " +
                "from " + DatabaseDefn.DBSchool + " " +
                "where schoolid = @id";

        public static string GetSchoolByAbbr =
                "select schoolid, schoolname, schoolpassword, principal, area, loc, phone, " +
                " username, school_abbr, clusternum " +
                "from " + DatabaseDefn.DBSchool + " " +
                "where school_abbr = \'@abbr\'";

        public static string GetAllSchools =
                "select schoolid, schoolname, schoolpassword, principal, area, loc, phone, " +
                " username, school_abbr, clusternum " +
                "from " + DatabaseDefn.DBSchool + " ";


        #endregion


        // ***** get tests
        #region get_tests

        public static string GetTestListAllTests =
                "select test_id, test_year, test_month, start_datetime, end_datetime, language_version, " +
                " test_subject, test_title, test_grade, num_items, num_points, pass_num, commended_num, " +
                " comp_score, projection, test_template, custom_query_orig, test_id_nbr, " +
                " actual_start_date, actual_end_date, school_type, custom_query, sec_school_type " +
                "from " + DatabaseDefn.DBTestDefn + " " +
                "where to_date(start_datetime, 'MM/DD/YYYY') >= to_date('08/22/2011', 'MM/DD/YYYY') " +
                "and to_date(start_datetime, 'MM/DD/YYYY') <= current_date " +
                "and test_subject not in ('BALLOT','SAMPLE') " +
                "order by test_id desc";
        
        #endregion


        // ***** get test info
        #region get_test_info

        public static string GetTestInfoForTest =
                "select test_id, test_year, test_month, start_datetime, end_datetime, language_version, " +
                " test_subject, test_title, test_grade, num_items, num_points, pass_num, commended_num, " +
                " comp_score, projection, test_template, custom_query_orig, test_id_nbr, " +
                " actual_start_date, actual_end_date, school_type, custom_query, sec_school_type " +
                "from " + DatabaseDefn.DBTestDefn + " " +
                "where test_id = \'@testId\'";

        public static string GetTestInfoForAll =
                "select test_id, test_year, test_month, start_datetime, end_datetime, language_version, " +
                " test_subject, test_title, test_grade, num_items, num_points, pass_num, commended_num, " +
                " comp_score, projection, test_template, custom_query_orig, test_id_nbr, " +
                " actual_start_date, actual_end_date, school_type, custom_query, sec_school_type " +
                "from " + DatabaseDefn.DBTestDefn;

        #endregion


        // ***** get answer keys
        #region get_answer_keys

        public static string GetDistrictTestAnswerKey =
                "select test_id, item_num, answer, objective, teks, weight " +
                "from " + DatabaseDefn.DBAnswerKey + " " +
                "where test_id = \'@testId\' " +
                "and (objective  != " + Constants.AnswerKeyFillerObjective.ToString() + " " +
                "or teks != \'" + Constants.AnswerKeyFillerTEKS + "\') " +
                "and teks != \'" + Constants.ExcludeItemTEKSInd + "\' " +
                "order by item_num asc";

        public static string GetCampusTestAnswerKey =
                "select test_id, school_abbr, item_num, answer, objective, teks, weight " +
                "from " + DatabaseDefn.DBAnswerKeyCampus + " " +
                "where test_id = \'@testId\' " +
                "and school_abbr = \'@schoolAbbr\' " +
                "order by item_num asc";

        public static string GetAllDistrictAnswerKeys =
                "select test_id, item_num, answer, objective, teks, weight " +
                "from " + DatabaseDefn.DBAnswerKey + " " +
                "order by test_id, item_num";

        public static string GetAllCampusAnswerKeys =
                "select test_id, school_abbr, item_num, answer, objective, teks, weight " +
                "from " + DatabaseDefn.DBAnswerKeyCampus + " " +
                "order by test_id, school_abbr, item_num";

        #endregion


        // ***** get scans
        #region get_scans

        public static string GetScansForTest =
                "select date_scanned, scanned_sequence, imagepath, name, student_id, " +
                "test_id, language_version, exempt, preslugged, answers " +
                "from " + DatabaseDefn.DBScans + " " +
                "where test_id = \'@testId\'";

        public static string GetScansForTestCampus =
                "select date_scanned, scanned_sequence, imagepath, name, student_id, test_id, " +
                " language_version, exempt, preslugged, answers " +
                "from " + DatabaseDefn.DBScans + " " +
                "where student_id in " + 
                "( select unique(student_id)  " +
                "from " + DatabaseDefn.DBScans + " " +
                "join " + DatabaseDefn.DBStudentRoster + " " +
                "on student_id = local_student_id " +
                "where test_id = nvl(\'@testId\',uid) " +
                "and school2 = \'@campus\' ) " +
                "and test_id = nvl(\'@testId\',uid) " +
                "order by student_id";

        #endregion


        // ***** get Roster data
        #region get_roster_data
        public static string GetRosterDataForID =
                "select student_name, local_student_id, lep_code, special_ed_flag, grade_level, " +
                " state_school_id, teacher_nbr, teacher_name, active, local_course_id, " +
                " district_course_title, semester, period, school_abbr, us_yrs, first_year, " +
                " benchmark_mod, school2 " +
                "from " + DatabaseDefn.DBStudentRoster + " " +
                "where local_student_id = \'@studentId\'";

        public static string GetAbbreviatedRosterDataForSchool =
                "SELECT LOCAL_STUDENT_ID, TEACHER_NAME, PERIOD, SEMESTER, SCHOOL2, " +
                "\' \' AS STUDENT_NAME, \' \' AS LEP_CODE, " +
                "\' \' AS SPECIAL_ED_FLAG, \' \' AS GRADE_LEVEL, " +
                "\' \' AS STATE_SCHOOL_ID, \' \' AS TEACHER_NBR, " +
                "\' \' AS ACTIVE, \' \' AS LOCAL_COURSE_ID, " +
                "\' \' AS DISTRICT_COURSE_TITLE, \' \' AS SCHOOL_ABBR, " +
                "\' \' AS US_YRS, \' \' AS FIRST_YEAR, \' \' AS BENCHMARK_MOD " +
                "from " + DatabaseDefn.DBStudentRoster + " " +
                "where school2 = \'@school\' ";

        public static string GetCoursesForTest = 
                "select unique(local_course_id) " +
                "from ( @query ) "; 

        #endregion


        // ***** get special stuff
        #region get_special_stuff

        public static string GetElemStudentsNotInAttCourse =
                "select unique(local_student_id), student_name, school2 " +
                "from ( " +
                " select local_student_id, school2, student_name " +
                " from " + DatabaseDefn.DBStudentRoster + " " +
                " where grade_level in (" + Constants.ElemGradeListForQuery + ") " +
                " and local_student_id not in ( " +
                "  select local_student_id " +
                "  from " + DatabaseDefn.DBStudentRoster + " " +
                "  where LOCAL_COURSE_ID in (" + Constants.ElemAttCourseListForQuery + ") " +
                " ) ) " +
                "where local_student_id = @studentId " +
                "group by local_student_id, student_name, school2 ";

        public static string MatchStudentToTeacherList =
                "select local_student_id, student_name, " +
                Constants.TeacherNameFieldName + ", " +
                "period, local_course_id, school2, \'@testId\' as test_id " +
                "from " + DatabaseDefn.DBStudentRoster + " r " +
                "where local_student_id = \'@studentId\' " +
                "and " + Constants.TeacherNameFieldName + " in (@teacherList) " +
                "order by " + Constants.TeacherNameFieldName + ", period";

        #endregion


    }
}