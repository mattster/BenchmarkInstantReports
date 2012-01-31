
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
            "select school_abbr, schoolname from " + DatabaseDefn.DBSchool + " " +
            "where schoolid between 1 and 9 or schoolid = 52 order by schoolname";

        public static string GetJHCampuses =
                "select school_abbr, schoolname from " + DatabaseDefn.DBSchool + " " +
                "where (schoolid between 10 and 99 and schoolid != 52) or schoolid = 6 order by schoolname";

        public static string GetELCampuses =
                "select school_abbr, schoolname from " + DatabaseDefn.DBSchool + " " +
                "where schoolid between 100 and 999 or schoolid = 6 order by schoolname";

        public static string GetCampusList =
                "select * from (" + GetHSCampuses + ") " +
                "union all " +
                "select * from (" + GetSeparator + ") " +
                "union all " +
                "select * from (" + GetJHCampuses + ") " +
                "union all " +
                "select * from (" + GetSeparator + ") " +
                "union all " +
                "select * from (" + GetELCampuses + ") ";

        public static string GetCampusInfoForCampus =
                "select school_abbr, schoolname from " + DatabaseDefn.DBSchool + " " +
                "where school_abbr = \'@schoolAbbr\' ";

        public static string GetSchoolID =
                "select schoolid from " + DatabaseDefn.DBSchool + " " +
                "where school_abbr = \'@schoolAbbr\'";

        public static string GetSchoolByID =
                "select * from " + DatabaseDefn.DBSchool + " " +
                "where schoolid = @id";

        public static string GetSchoolByAbbr =
                "select * from " + DatabaseDefn.DBSchool + " " +
                "where school_abbr = \'@abbr\'";

        public static string GetAllSchools =
                "select * from " + DatabaseDefn.DBSchool + " ";


        #endregion


        // ***** get tests
        #region get_tests

        public static string GetTestListBySchoolTypes =
                "select test_id from " + DatabaseDefn.DBTestDefn + " " +
                "where school_type in (@schoolTypeList) " +
                "and to_date(start_datetime, 'MM/DD/YYYY') >= to_date('08/22/2011', 'MM/DD/YYYY') " +
                "and to_date(start_datetime, 'MM/DD/YYYY') <= current_date " +
                "and test_subject not in ('BALLOT','SAMPLE') " +
                "order by test_id desc";

        public static string GetTestListAllTests =
                "select * from " + DatabaseDefn.DBTestDefn + " " +
                "where to_date(start_datetime, 'MM/DD/YYYY') >= to_date('08/22/2011', 'MM/DD/YYYY') " +
                "and to_date(start_datetime, 'MM/DD/YYYY') <= current_date " +
                "and test_subject not in ('BALLOT','SAMPLE') " +
                "order by test_id desc";

        public static string GetTestListBySecSchoolType =
                "select test_id from " + DatabaseDefn.DBTestDefn + " " +
                "where sec_school_type = \'@schoolType\' " +
                "and to_date(start_datetime, 'MM/DD/YYYY') >= to_date('08/22/2011', 'MM/DD/YYYY') " +
                "and to_date(start_datetime, 'MM/DD/YYYY') <= current_date " +
                "and test_subject not in ('BALLOT','SAMPLE') " +
                "order by test_id desc";

        public static string GetTestListBySchoolTypeSec =
                "select test_id from " + DatabaseDefn.DBTestDefn + " " +
                "where sec_school_type in(\'@schoolType\','S') " +
                "and to_date(start_datetime, 'MM/DD/YYYY') >= to_date('08/22/2011', 'MM/DD/YYYY') " +
                "and to_date(start_datetime, 'MM/DD/YYYY') <= current_date " +
                "and test_subject not in ('BALLOT','SAMPLE') " +
                "order by test_id desc";
        
        #endregion


        // ***** get test info
        #region get_test_info

        public static string GetTestInfoForTest =
                "select * from " + DatabaseDefn.DBTestDefn + " " +
                "where test_id = \'@testId\'";

        public static string GetTestInfoForAll =
                "select * from " + DatabaseDefn.DBTestDefn;

        public static string GetNumTestQuestions =
                "select max(item_num) from " + DatabaseDefn.DBTestDefn + " " +
                "where test_id = \'@testId\'";

        public static string GetTestType =
                "select school_type from " + DatabaseDefn.DBTestDefn + " " +
                "where test_id = \'@testId\' ";

        public static string GetCustomQuery =
                "select custom_query from " + DatabaseDefn.DBTestDefn + " " +
                "where test_id = \'@testId\'";

        public static string GetPassNum =
                "select pass_num from " + DatabaseDefn.DBTestDefn + " " +
                "where test_id = \'@testId\'";

        public static string GetCommendedNum =
                "select commended_num from " + DatabaseDefn.DBTestDefn + " " +
                "where test_id = \'@testId\'";
        
        public static string GetTestTemplate =
                "select test_template from " + DatabaseDefn.DBTestDefn + " " +
                "where test_id = \'@testId\'";

        #endregion


        // ***** get answer keys
        #region get_answer_keys

        public static string GetDistrictTestAnswerKey =
                "select * FROM " + DatabaseDefn.DBAnswerKey + " " +
                "where test_id = \'@testId\' " +
                "and (objective  != " + Constants.AnswerKeyFillerObjective.ToString() + " " +
                "or teks != \'" + Constants.AnswerKeyFillerTEKS + "\') " +
                "and teks != \'" + Constants.ExcludeItemTEKSInd + "\' " +
                "order by item_num asc";

        public static string GetCampusTestAnswerKey =
                "select * FROM " + DatabaseDefn.DBAnswerKeyCampus + " " +
                "where test_id = \'@testId\' " +
                "and school_abbr = \'@schoolAbbr\' " +
                "and item_num >= @itemNumStart " +
                "and item_num <= @itemNumEnd " +
                "order by item_num asc";

        public static string GetAllDistrictAnswerKeys =
                "select * from " + DatabaseDefn.DBAnswerKey + " " +
                "order by test_id, item_num";

        public static string GetAllCampusAnswerKeys =
                "select * from " + DatabaseDefn.DBAnswerKeyCampus + " " +
                "order by test_id, school_abbr, item_num";

        #endregion


        // ***** get scans
        #region get_scans

        public static string GetLatestScanForStudent = 
                "select * from (select * from " + DatabaseDefn.DBScans + " " +
                "where student_id = \'@studentId\' and test_id = \'@testId\' " +
                "order by to_date(date_scanned, 'MM/DD/YYYY HH:MI:SS AM') DESC ) " +
                "where rownum = 1";

        public static string GetAllScansForTest =
                "SELECT * FROM " + DatabaseDefn.DBScans + " " +
                "WHERE TEST_ID IN (@testIdList) " +
                "ORDER BY STUDENT_ID ASC";

        public static string GetScansForCampus =
                "select unique student_id, test_id  " +
                "from " + DatabaseDefn.DBScans + " b " +
                "join " + DatabaseDefn.DBStudentRoster + " r " +
                "on student_id = local_student_id " +
                "where test_id = nvl(\'@testId\',uid) " +
                "and school2 = \'@campus\' " +
                "order by student_id asc";

        public static string GetStudentScansForCampusCourse =
                "select local_student_id, student_name, " +
                Constants.TeacherNameFieldName + ", " +
                "period, local_course_id, school2, test_id " +
                "from ( " +
                "select STUDENT_ID, TEST_ID from " + DatabaseDefn.DBScans + " " +
                "where test_id = \'@testId\' ) " +
                "join ( @testQuery ) " +
                "on student_id = local_student_id ";

        public static string GetStudentScansFromTestQuery =
                "select local_student_id, student_name, " +
                Constants.TeacherNameFieldName + ", " +
                "period, local_course_id, school2, test_id " +
                "from ( " +
                "select STUDENT_ID, TEST_ID from " + DatabaseDefn.DBScans + " " +
                "where test_id = \'@testId\' ) " +
                "join ( @query ) " +
                "on student_id = local_student_id " +
                "where @teacherQuery " +
                "and @periodQuery " +
                "order by STATE_SCHOOL_ID, TEACHER_NAME, PERIOD, STUDENT_NAME ";

        // optimized query from Norma
        public static string GetStudentsWithScansNotInTestCriteria =
                "select /*+ no_cpu_costing */ unique student_id, test_id " +
                "from " + DatabaseDefn.DBScans + " b, " + DatabaseDefn.DBStudentRoster + " r1 " +
                "where test_id = nvl(\'@testId\',uid) " +
                "and school2 = \'@campus\' " +
                "and student_id not in ( " +
                "select unique local_student_id from ( " +
                " @query) ) " +
                " and b.student_id = r1.local_student_id " +
                "order by student_id asc";

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

        public static string GetTeachersForTest =
                "select unique " + Constants.TeacherNameFieldName + ", period " +
                "from ( @query ) " +
                "group by " + Constants.TeacherNameFieldName + ", period " +
                "order by " + Constants.TeacherNameFieldName + ", period ";


        #endregion


    }
}