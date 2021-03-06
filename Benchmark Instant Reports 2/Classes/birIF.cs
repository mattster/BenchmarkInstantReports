﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Benchmark_Instant_Reports_2.Exceptions;
using Benchmark_Instant_Reports_2.Grading;
//using DevExpress.Web.ASPxEditors;



//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
// an interface class for all the Benchmark Instant Reports
//
// matt hollingsworth
// oct 2010
//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *



namespace Benchmark_Instant_Reports_2
{
    public class birIF
    {

        #region birconstants
        public static int maxNumTestQuestions = 200;
        public static string birExcludeItemAnsInd = "X";
        public static string birExcludeItemTEKSInd = "XXXXX";
        public static bool specialDebug = false;                         // normally false
        public static bool lookupStudentNamesFromRoster = true;        // normally true
        public static bool demoMode = false;                             // normally false
        public static int testTemplTypeNoGrids = 0;
        public static int testTemplType60_3GridEOC = 1;
        public static int testTemplType60_3GridG68 = 2;
        public static int testTemplType28_2GridG68 = 3;
        public static int testTemplType29_1GridG68 = 4;
        public static int testTemplType19_1GridEOC = 5;
        public static int testTemplType18_2GridEOC = 6;
        public static int testTemplType24_1GridG45 = 7;
        public static string unkTeacherName = "zUNKNOWN TEACHER";
        public static string allTeachers = "__All Teachers__";
        public static string usernameAllCampuses = "ALL";

        #endregion

        #region birdbnames
        public static string dbScans = "ACI.BENCHMARK";
        public static string dbTestDefn = "ACI.TEST_DEFINITION";
        public static string dbSchool = "ACI.SCHOOL";
        public static string dbAnswerKey = "ACI.ANSWER_KEY";
        public static string dbAnswerKeyCampus = "ACI.ANSWER_KEY_CAMPUS";
        public static string dbObjectives = "ACI.OBJECTIVES";

        public static string dbStudentRoster_reg = "SIS_ODS.RISD_STUDENT_ROSTER_VW";
        public static string dbStudentRoster_temp = "ACI.ROSTER_TEMP";
        public static string dbStudentRoster = demoMode ? dbStudentRoster_temp : dbStudentRoster_reg;

        public static string dbResultsBenchmarkStats = "ACI.TEMP_RESULTS_BENCHMARKSTATS";
        public static string dbResultsScanReport = "ACI.TEMP_RESULTS_SCANREPORT";
        public static string dbResultsStudentStats = "ACI.TEMP_RESULTS_STUDENTSTATS";
        #endregion


        #region birstrings

        public static string dropDownSeparatorString = "----";
        public static string teacherNameNumFieldName = "R.TEACHER_NAME||\'-\'||R.TEACHER_NBR";
        public static string teacherNameFieldNameR = "R.TEACHER_NAME";
        public static string teacherNameFieldName = "TEACHER_NAME";
        public static string teacherNameNumFieldNameNew = "TEACHER_NAMENUM";
        public static string elemGradeListForQuery = "\'01\',\'02\',\'03\',\'04\',\'05\',\'06\'";
        public static string elemAttCourseListForQuery = "\'E9001\',\'E9002\',\'E9003\',\'E9004\',\'E9005\',\'E9006\'";
        public static int answerKeyFillerObjective = 99;
        public static string answerKeyFillerTEKS = "ZZZZZ";
        public static string testTemplate60Q3GridEOC = "60-AE-FK-3GRID-EOC";
        public static string testTemplate60Q3GridG68 = "60-AE-FK-3GRID-G68";
        public static string testTemplate28Q2GridG68 = "28-AE-FK-2GRID-G68";
        public static string testTemplate29Q1GridG68 = "29-AE-FK-1GRID-G68";
        public static string testTemplate18Q2GridEOC = "18-AE-FK-2GRID-EOC";
        public static string testTemplate19Q1GridEOC = "19-AE-FK-1GRID-EOC";
        public static string testTemplate24Q1GridG45 = "24-AE-FK-1GRID-G45";

        public static string schoolCriteriaInCustomQ = "AND R.SCHOOL2 = @school"; // "AND R.SCHOOL_ABBR = @school";
        public static string schoolCriteriaInCustomQ2 = "AND SCHOOL2 = @school"; //"AND SCHOOL_ABBR = @school";
        public static string schoolCriteriaInCustomQ3 = "and school2 = @school"; //"and school_abbr = @school";
        public static string schoolCriteriaInCustomQ4 = "AND R.SCHOOL_ABBR = @school";
        public static string schoolCriteriaInCustomQ5 = "AND SCHOOL_ABBR = @school";
        public static string schoolCriteriaInCustomQ6 = "and school_abbr = @school";

        #endregion

        #region queries

        private static string getHSCampusQuery =
                "select school_abbr, schoolname from aci.school " +
                "where schoolid between 1 and 9 or schoolid = 52 order by schoolname";

        private static string getJHCampusQuery =
                "select school_abbr, schoolname from aci.school " +
                "where (schoolid between 10 and 99 and schoolid != 52) or schoolid = 6 order by schoolname";

        private static string getELCampusQuery =
                "select school_abbr, schoolname from aci.school " +
                "where schoolid between 100 and 999 or schoolid = 6 order by schoolname";

        private static string getseparatorQuery =
                "select cast('" + dropDownSeparatorString + "' as nvarchar2(4)) as school_abbr, " +
                "cast('" + dropDownSeparatorString + "' as varchar2(50)) as schoolname from dual";

        public static string getCampusListQuery =
                "select * from (" + getHSCampusQuery + ") " +
                "union all " +
                "select * from (" + getseparatorQuery + ") " +
                "union all " +
                "select * from (" + getJHCampusQuery + ") " +
                "union all " +
                "select * from (" + getseparatorQuery + ") " +
                "union all " +
                "select * from (" + getELCampusQuery + ") ";

        public static string getCampusInfoForCampus =
                "select school_abbr, schoolname from aci.school " +
                "where school_abbr = \'@schoolAbbr\' ";

        // TEMPCHANGE for the demo
        //public static string getCampusListQuery =
        //           "select * from (" + getJHCampusQuery + ") ";

        public static string getBenchmarkListQuery =
                "select test_id from " + dbTestDefn + " " +
                "where school_type in (@schoolTypeList) " +
            //"and current_date <= add_months(to_date(end_datetime, 'MM/DD/YYYY')) " +
                "and to_date(start_datetime, 'MM/DD/YYYY') >= to_date('08/22/2011', 'MM/DD/YYYY') " +
                "and to_date(start_datetime, 'MM/DD/YYYY') <= current_date " +
                "and test_subject not in ('BALLOT','SAMPLE') " +
                "order by test_id desc";

        public static string queryGetTestListBySchoolType =
                "select test_id from " + dbTestDefn + " " +
                "where sec_school_type = \'@schoolType\' " +
                "and to_date(start_datetime, 'MM/DD/YYYY') >= to_date('08/22/2011', 'MM/DD/YYYY') " +
                "and to_date(start_datetime, 'MM/DD/YYYY') <= current_date " +
                "and test_subject not in ('BALLOT','SAMPLE') " +
                "order by test_id desc";

        public static string queryGetTestListBySchoolTypeSec =
                "select test_id from " + dbTestDefn + " " +
                "where sec_school_type in(\'@schoolType\','S') " +
                "and to_date(start_datetime, 'MM/DD/YYYY') >= to_date('08/22/2011', 'MM/DD/YYYY') " +
                "and to_date(start_datetime, 'MM/DD/YYYY') <= current_date " +
                "and test_subject not in ('BALLOT','SAMPLE') " +
                "order by test_id desc";


        public static string queryGetTestListAllTests =
                "select test_id from " + dbTestDefn + " " +
                "where to_date(start_datetime, 'MM/DD/YYYY') >= to_date('08/22/2011', 'MM/DD/YYYY') " +
                "and to_date(start_datetime, 'MM/DD/YYYY') <= current_date " +
                "and test_subject not in ('BALLOT','SAMPLE') " +
                "order by test_id desc";


        // TEMPCHANGE temporary thing
        //public static string getBenchmarkListQuery = 
        //        "select test_id from " + dbTestDefn + " " +
        //        "where school_type in (@schoolTypeList) " + 
        //        "and test_id like '2011-04%EOC%' " +
        //        "order by test_id desc";


        public static string latestScanQuery = "select * from (select * from aci.benchmark " +
                "where student_id = \'@studentId\' and test_id = \'@testId\' " +
                "order by to_date(date_scanned, 'MM/DD/YYYY HH:MI:SS AM') DESC ) " +
                "where rownum = 1";

        public static string elemStudentNotInAttCourseQuery =
            "select unique(local_student_id), student_name, school2 " +
            "from ( " +
            " select local_student_id, school2, student_name " +
            " from " + dbStudentRoster + " " +
            " where grade_level in (" + elemGradeListForQuery + ") " +
            " and local_student_id not in ( " +
            "  select local_student_id " +
            "  from " + dbStudentRoster + " " +
            "  where LOCAL_COURSE_ID in (" + elemAttCourseListForQuery + ") " +
            " ) ) " +
            "where local_student_id = @studentId " +
            "group by local_student_id, student_name, school2 ";

        public static string numTestQuestionsQuery = "select * from ( " +
                "select item_num from aci.answer_key " +
                "where test_id = \'@testId\' order by item_num desc ) " +
                "where rownum = 1";

        public static string testTypeQuery =
                "select school_type from " + dbTestDefn + " " +
                "where test_id = \'@testId\' ";


        public static string getDistrictTestAnswerKeyQuery =
                "select * FROM " + dbAnswerKey + " " +
                "where test_id = \'@testId\' " +
                "and (objective  != " + answerKeyFillerObjective.ToString() + " " +
                "or teks != \'" + answerKeyFillerTEKS + "\') " +
                "and teks != \'" + birExcludeItemTEKSInd + "\' " +
                "order by item_num asc";

        public static string getCampusTestAnswerKeyQuery =
                "select * FROM " + dbAnswerKeyCampus + " " +
                "where test_id = \'@testId\' " +
                "and school_abbr = \'@schoolAbbr\' " +
                "and item_num >= @itemNumStart " +
                "and item_num <= @itemNumEnd " +
                "order by item_num asc";

        public static string getStudentScansQuery =
                "SELECT * FROM ACI.BENCHMARK  " +
                "WHERE TEST_ID IN (@testIdList) " +
                "ORDER BY STUDENT_ID ASC";

        public static string getStudentScansIdsQuery =
                "SELECT UNIQUE TO_CHAR(STUDENT_ID, 'TM9') AS STUDENT_ID " +
                "FROM ACI.BENCHMARK " +
                "WHERE TEST_ID IN (@testIdList) " +
                "ORDER BY STUDENT_ID ASC";

        public static string getStudentScansFromCampusQuery =
                "select unique student_id, test_id  " +
                "from " + dbScans + " b " +
                "join " + dbStudentRoster + " r " +
                "on student_id = local_student_id " +
            //"where test_id in (@testIdList) " +
                "where test_id = nvl(\'@testId\',uid) " +
                "and school2 = \'@campus\' " +
                "order by student_id asc";

        public static string getCustQueryQuery =
                "select custom_query " +
                "from " + dbTestDefn + " " +
                "where test_id = \'@testId\'";

        public static string queryGetStudentScansFromCampusCourse =
                "select local_student_id, student_name, " +
                teacherNameFieldName + ", " +
                "period, local_course_id, school2, test_id " +
                "from ( " +
                "select STUDENT_ID, TEST_ID from " + dbScans + " " +
                "where test_id = \'@testId\' ) " +
                "join ( @testQuery ) " +
                "on student_id = local_student_id ";


        public static string queryGetStudentScansFromCampusTeacher =
                "select local_student_id, student_name, " +
                teacherNameFieldName + ", " +
                "period, local_course_id, school2, test_id " +
                "from ( " +
                "select STUDENT_ID, TEST_ID from " + dbScans + " " +
                "where test_id = \'@testId\' ) " +
                "join ( select * from " + dbStudentRoster + " " +
                "where school2 = \'@campus\' " +
                "@teacherQuery " +
                "@periodQuery " +
                "@semesterQuery " +
                "order by STATE_SCHOOL_ID, TEACHER_NAME, PERIOD, STUDENT_NAME ) " +
                "on student_id = local_student_id ";

        public static string queryGetStudentScansFromTestQuery =
                "select local_student_id, student_name, " +
                teacherNameFieldName + ", " +
                "period, local_course_id, school2, test_id " +
                "from ( " +
                "select STUDENT_ID, TEST_ID from " + dbScans + " " +
                "where test_id = \'@testId\' ) " +
                "join ( @query ) " +
                "on student_id = local_student_id " +
                "where @teacherQuery " +
                "and @periodQuery " +
                "order by STATE_SCHOOL_ID, TEACHER_NAME, PERIOD, STUDENT_NAME ";

        //public static string queryGetStudentsWithScansNotInTestCriteria =
        //        "select unique student_id, test_id  " +
        //        "from " + dbScans + " b " +
        //        "join " + dbStudentRoster + " r " +
        //        "on student_id = local_student_id " +
        //        "where test_id = nvl(\'@testId\',uid) " +
        //        "and school2 = \'@campus\' " +
        //        "and student_id not in ( " +
        //        "select unique local_student_id from ( " +
        //        " @query " +
        //        " )  ) " +
        //        "order by student_id asc";

        //optimized query from Norma
        public static string queryGetStudentsWithScansNotInTestCriteria =
                "select /*+ no_cpu_costing */ unique student_id, test_id " +
                "from " + dbScans + " b, " + dbStudentRoster + " r1 " +
                "where test_id = nvl(\'@testId\',uid) " +
                "and school2 = \'@campus\' " +
                "and student_id not in ( " +
                "select unique local_student_id from ( " +
                " @query) ) " +
                " and b.student_id = r1.local_student_id " +
                "order by student_id asc";

        public static string queryMatchStudentToTeacherList =
                "select local_student_id, student_name, " +
                teacherNameFieldName + ", " +
                "period, local_course_id, school2, \'@testId\' as test_id " +
                "from " + dbStudentRoster + " r " +
                "where local_student_id = \'@studentId\' " +
                "and " + teacherNameFieldName + " in (@teacherList) " +
                "order by " + teacherNameFieldName + ", period";

        public static string queryGetTeachersForTest =
                "select unique " + teacherNameFieldName + ", period " +
                "from ( @query ) " +
                "group by " + teacherNameFieldName + ", period " +
                "order by " + teacherNameFieldName + ", period ";

        public static string queryGetTeacherPeriodsForTest =
                "select unique " + teacherNameFieldName + ", period " +
                "from ( @query ) " +
                "where " + teacherNameFieldName + " = \'@teacher\' " +
                "group by " + teacherNameFieldName + ", period " +
                "order by " + teacherNameFieldName + ", period ";

        public static string queryGetStudentScansFromBenchmarkOnly =
                "select student_id as local_student_id, name, " +
                "\'ALL\' as " + teacherNameFieldName + ", " +
                "\'ALL\' as period, " +
                "\'ALL\' as local_course_id, " +
                "\'ALL\' as school2, test_id " +
                "from " + dbScans + " " +
                "where test_id = \'@testId\' ";

        public static string queryGetStudentRosterDataForTest =
            "select * " +
            "from " + dbStudentRoster + " r " +
            "where local_student_id = \'@studentID\' " +
            "and local_course_id in (@courses) ";

        public static string queryGetPassNum =
                "select pass_num " +
                "from " + dbTestDefn + " " +
                "where test_id = \'@testId\'";

        public static string queryGetCommendedNum =
                "select commended_num " +
                "from " + dbTestDefn + " " +
                "where test_id = \'@testId\'";

        //public static string queryGetTestTemplate =
        //        "select test_template " +
        //        "from " + dbTestDefn + " " +
        //        "where test_id = \'@testId\'";

        public static string queryGetSchoolID =
                "select schoolid " +
                "from " + dbSchool + " " +
                "where school_abbr = \'@schoolAbbr\'";

        #endregion


        //**********************************************************************//
        //** returns an array of course id's that are applicable to
        //** the given test
        public static string[] getCourseIDsForTest(string testId)
        {
            string custQueryQs = getCustQueryQuery.Replace("@testId", testId);

            DataSet dsGetQuery = dbIFOracle.getDataRows(custQueryQs);

            // remove the "@school" portion of the custom query
            string modifiedCustomQuery = dsGetQuery.Tables[0].Rows[0][0].ToString().Replace(
                schoolCriteriaInCustomQ, "");
            modifiedCustomQuery = modifiedCustomQuery.Replace(schoolCriteriaInCustomQ2, "");
            modifiedCustomQuery = modifiedCustomQuery.Replace(schoolCriteriaInCustomQ3, "");
            modifiedCustomQuery = modifiedCustomQuery.Replace(schoolCriteriaInCustomQ4, "");
            modifiedCustomQuery = modifiedCustomQuery.Replace(schoolCriteriaInCustomQ5, "");
            modifiedCustomQuery = modifiedCustomQuery.Replace(schoolCriteriaInCustomQ6, "");

            DataSet dsCustomQuery = dbIFOracle.getDataRows(modifiedCustomQuery);

            return birUtilities.getUniqueTableColumnStringValues(dsCustomQuery.Tables[0], "LOCAL_COURSE_ID");
        }


        //**********************************************************************//
        //** returns a string array of list of student ID's that have 
        //** scanned answer sheets in the benchmark table for the given
        //** test_id and campus
        //**
        //public static string[] getListOfStudentsWithScans(string[] benchmarks, string campus)
        //{
        //    string qs = getStudentScansFromCampusQuery.Replace("@campus", campus);
        //    qs = qs.Replace("@testIdList", birUtilities.convertStringArrayForQuery(benchmarks));

        //    DataSet ds = dbIFOracle.getDataRows(qs);

        //    return birUtilities.getUniqueTableColumnStringValues(ds.Tables[0], "STUDENT_ID");
        //}


        //**********************************************************************//
        //** returns a DataSet with the list of students from the selected
        //** campus that have scans for the specified test; data includes some
        //** matching data from the student roster
        //**
        public static DataSet getStudentScanListData(string benchmark, string campus)
        {
            if (campus == "ALL Elementary" || campus == "ALL Secondary")
            {
                string qs = queryGetStudentScansFromCampusCourse.Replace("@testId", benchmark);
                qs = qs.Replace("@testQuery", GetRawCustomQuery(benchmark));
                qs = qs.Replace("AND R.SCHOOL2 = @school", " ");
                qs = qs.Replace("AND SCHOOL2 = @school", " ");
                qs = qs.Replace("AND SCHOOL_ABBR = @school", " ");
                qs = qs.Replace("AND school_abbr = @school", " ");
                qs = qs.Replace("and school_abbr = @school", " ");
                qs = qs.Replace("R.GRADE_LEVEL = \'10\'", "R.GRADE_LEVEL != \'0\'");
                qs = qs.Replace("R.GRADE_LEVEL = \'11\'", "R.GRADE_LEVEL != \'0\'");

                return makeUniqueDataSet(dbIFOracle.getDataRows(qs));

            }

            else
            {
                string qs = queryGetStudentScansFromCampusCourse.Replace("@testId", benchmark);
                qs = qs.Replace("@testQuery", GetRawCustomQuery(benchmark));
                qs = qs.Replace("@campus", campus);
                qs = qs.Replace("@school", "\'" + campus + "\'");
                qs = qs.Replace("R.GRADE_LEVEL = \'10\'", "R.GRADE_LEVEL != \'0\'");
                qs = qs.Replace("R.GRADE_LEVEL = \'11\'", "R.GRADE_LEVEL != \'0\'");

                return makeUniqueDataSet(dbIFOracle.getDataRows(qs));
            }
        }


        public static DataSet getTeachersForTestCampus(string benchmark, string campus)
        {
            string customQuery = GetRawCustomQuery(benchmark);
            customQuery = customQuery.Replace("@school", "\'" + campus + "\'");

            string qs = queryGetTeachersForTest.Replace("@query", customQuery);

            return dbIFOracle.getDataRows(qs);
        }

        public static DataSet getTeachersForTestCampus(string benchmark, string campus, string teacher)
        {
            //string customQuery = GetRawCustomQuery(benchmark);
            //customQuery = customQuery.Replace("@school", "\'" + campus + "\'");

            //string qs = queryGetTeacherPeriodsForTest.Replace("@query", customQuery);
            //qs = qs.Replace("@teacher", teacher);

            //return dbIFOracle.getDataRows(qs);

            //DataSet ds = getStudentDataToGrade(benchmark, campus, teacher);
            ////string selectTeachersPeriods = "teacher_name, period";
            //DataView dv = new DataView(ds.Tables[0]);
            //DataTable uniqueTable = dv.ToTable(true, "TEACHER_NAME", "PERIOD");
            //DataSet dsReturn = new DataSet();
            //dsReturn.Tables.Add(uniqueTable);

            DataSet dsTeachers = getTeachersForTestCampus(benchmark, campus);
            DataTable dtThisTeacher = dsTeachers.Tables[0].Clone();

            foreach (DataRow row in dsTeachers.Tables[0].Rows)
            {
                if (row[teacherNameFieldName].ToString() == teacher)
                {
                    DataRow newrow = dtThisTeacher.NewRow();
                    newrow.ItemArray = row.ItemArray;
                    dtThisTeacher.Rows.Add(newrow);
                }
            }

            DataSet dsReturn = new DataSet();
            dsReturn.Tables.Add(dtThisTeacher);

            return dsReturn;
        }

        public static DataSet getStudentScanListData2(string benchmark, string campus)
        {
            string customQuery = GetRawCustomQuery(benchmark);
            customQuery = customQuery.Replace("@school", "\'" + campus + "\'");

            string qs = queryGetStudentScansFromTestQuery.Replace("@testId", benchmark);
            qs = qs.Replace("@query", customQuery);
            qs = qs.Replace("@teacherQuery", "1=1 ");
            qs = qs.Replace("@periodQuery", "1=1 ");

            return makeUniqueDataSet(dbIFOracle.getDataRows(qs));
        }

        public static DataSet getStudentScanListData(string benchmark)
        {
            string qs = queryGetStudentScansFromBenchmarkOnly.Replace("@testId", benchmark);

            return makeUniqueDataSet(dbIFOracle.getDataRows(qs));
        }

        public static DataSet getStudentScanListData(string benchmark, string campus, string teacher)
        {
            string customQuery = GetRawCustomQuery(benchmark);
            customQuery = customQuery.Replace("@school", "\'" + campus + "\'");

            string qs = queryGetStudentScansFromTestQuery.Replace("@testId", benchmark);
            qs = qs.Replace("@query", customQuery);
            qs = qs.Replace("@campus", "\'" + campus + "\'");
            string qTeacher = teacher.Replace("'", "''");
            qs = qs.Replace("@teacherQuery", "teacher_name = \'" + qTeacher + "\' ");
            qs = qs.Replace("@periodQuery", "1=1 ");

            return makeUniqueDataSet(dbIFOracle.getDataRows(qs));
        }


        public static DataSet getStudentScanListData(string benchmark, string campus, string teacher,
            string periodList)
        {
            string customQuery = GetRawCustomQuery(benchmark);
            customQuery = customQuery.Replace("@school", "\'" + campus + "\'");

            string qs = queryGetStudentScansFromTestQuery.Replace("@testId", benchmark);
            qs = qs.Replace("@query", customQuery);
            string qTeacher = teacher.Replace("'", "''");
            qs = qs.Replace("@teacherQuery", "teacher_name = \'" + qTeacher + "\' ");
            qs = qs.Replace("@periodQuery", "period in (" + periodList + ") ");

            return makeUniqueDataSet(dbIFOracle.getDataRows(qs));
        }


        public static DataSet getStudentDataToGrade(string benchmark)
        {
            string[] schoollist;
            DataSet dsCurData = new DataSet();
            DataSet dsFinalData = new DataSet();

            // get a student data dataset for each campus and merge them together
            if (isTestElementary(benchmark))
                schoollist = getElemAbbrList();
            else
                schoollist = getSecAbbrList();

            int progress = (int)(100 / schoollist.Count());

            foreach (string campus in schoollist)
            {
                dsCurData = getStudentDataToGrade(benchmark, campus);
                if (dsFinalData.Tables.Count == 0)
                    dsFinalData.Tables.Add(dsCurData.Tables[0].Clone());
                dsFinalData.Tables[0].Merge(dsCurData.Tables[0]);
                //pbar.Position += progress;
            }

            return dsFinalData;
        }


        public static DataSet getStudentDataToGrade(string benchmark, string campus, string teacher = "",
            string periodList = "'00','01','02','03','04','05','06','07','08','09','10','11','12','13','14'")
        {
            DataSet dsReturn = new DataSet();
            List<string> studIdList1 = new List<string>();

            // get a dataset of the student scans for this test and campus
            string qs1 = getStudentScansFromCampusQuery.Replace("@campus", campus);
            qs1 = qs1.Replace("@testId", benchmark);
            DataSet dsStudentScansForCampus = dbIFOracle.getDataRows(qs1);

            // get a dataset of students who meet the criteria for this test
            string qs2 = getCustQueryQuery.Replace("@testId", benchmark);
            DataSet dsCustQuery = dbIFOracle.getDataRows(qs2);
            string qs3 = dsCustQuery.Tables[0].Rows[0][0].ToString().Replace("@school", "\'" + campus + "\'");
            DataSet dsPreslugDataForTest = dbIFOracle.getDataRows(qs3);
            string[] teacherList = birUtilities.getUniqueTableColumnStringValues(dsPreslugDataForTest.Tables[0], teacherNameFieldName);

            // get student data for students who have scans and meet the test criteria
            DataSet dsPresluggedStudentsWithScans = new DataSet();
            if (teacher == "")
                dsPresluggedStudentsWithScans = getStudentScanListData2(benchmark, campus);
            else
                dsPresluggedStudentsWithScans = getStudentScanListData(benchmark, campus, teacher, periodList);
            DataTable returnTable = dsPresluggedStudentsWithScans.Tables[0].Copy();

            // get student ID's for students who have scans but do not meet the test criteria
            string qs4 = queryGetStudentsWithScansNotInTestCriteria.Replace("@testId", benchmark);
            qs4 = qs4.Replace("@campus", campus);
            string customQuery = GetRawCustomQuery(benchmark);
            customQuery = customQuery.Replace("@school", "\'" + campus + "\'");
            qs4 = qs4.Replace("@query", customQuery);
            DataSet dsStudentsWithScansNotInTestCriteria = dbIFOracle.getDataRows(qs4);

            // for any students who do not match test criteria, try to match them up somehow
            if (dsStudentsWithScansNotInTestCriteria.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsStudentsWithScansNotInTestCriteria.Tables[0].Rows)
                {
                    string studentId = row["STUDENT_ID"].ToString();
                    if (studentId.Length == 5)              // add a leading 0 if necessary - makes things better
                        studentId = "0" + studentId;

                    if (birUtilities.isTeacherUnknown(studentId))
                    {
                        DataRow temprow = returnTable.NewRow();
                        temprow["LOCAL_STUDENT_ID"] = studentId;
                        temprow["STUDENT_NAME"] = "";
                        temprow[teacherNameFieldName] = unkTeacherName;
                        temprow["PERIOD"] = "01";
                        temprow["LOCAL_COURSE_ID"] = "00000";
                        temprow["SCHOOL2"] = campus;
                        temprow["TEST_ID"] = benchmark;
                        returnTable.Rows.Add(temprow);
                    }
                    else
                    {

                        // try to match them to teachers in the test query
                        string qs = queryMatchStudentToTeacherList.Replace("@testId", benchmark);
                        qs = qs.Replace("@studentId", studentId);
                        qs = qs.Replace("@teacherList", birUtilities.convertStringArrayForQuery(teacherList));
                        DataSet dsStudentMatches = dbIFOracle.getDataRows(qs);
                        if (dsStudentMatches.Tables[0].Rows.Count > 0)
                        {
                            DataRow temprow = returnTable.NewRow();
                            temprow.ItemArray = dsStudentMatches.Tables[0].Rows[0].ItemArray;
                            returnTable.Rows.Add(temprow);
                        }

                        else
                        {
                            // try to match them up to teachers when removing the BENCHMARK_MOD criteria
                            string customQueryNoMod = customQuery.Replace("AND BENCHMARK_MOD LIKE \'____1\'", " ");
                            customQueryNoMod = customQueryNoMod.Replace("AND BENCHMARK_MOD LIKE \'___1_\'", " ");
                            customQueryNoMod = customQueryNoMod.Replace("AND BENCHMARK_MOD LIKE \'__1__\'", " ");
                            customQueryNoMod = customQueryNoMod.Replace("AND BENCHMARK_MOD LIKE \'_1___\'", " ");
                            customQueryNoMod = customQueryNoMod.Replace("AND BENCHMARK_MOD LIKE \'1____\'", " ");
                            dsStudentMatches = dbIFOracle.getDataRows(customQueryNoMod);
                            if (dsStudentMatches.Tables[0].Rows.Count > 0)
                            {
                                string selectStudentFilter = "LOCAL_STUDENT_ID = \'" + studentId + "\'";
                                DataRow[] matchingRows = dsStudentMatches.Tables[0].Select(selectStudentFilter);
                                int c = matchingRows.Count();
                                if (matchingRows.Count() > 0)
                                {
                                    DataRow temprow = returnTable.NewRow();
                                    temprow["LOCAL_STUDENT_ID"] = matchingRows[0]["LOCAL_STUDENT_ID"];
                                    temprow["STUDENT_NAME"] = matchingRows[0]["STUDENT_NAME"];
                                    temprow[teacherNameFieldName] = matchingRows[0][teacherNameFieldName];
                                    temprow["PERIOD"] = matchingRows[0]["PERIOD"];
                                    temprow["LOCAL_COURSE_ID"] = matchingRows[0]["LOCAL_COURSE_ID"];
                                    temprow["SCHOOL2"] = matchingRows[0]["SCHOOL2"];
                                    temprow["TEST_ID"] = benchmark;
                                    returnTable.Rows.Add(temprow);
                                }

                                else
                                {
                                    // no matching teacher found in query removing BENCHMARK_MOD criteria, put "UNKNOWN"
                                    DataRow temprow = returnTable.NewRow();
                                    temprow["LOCAL_STUDENT_ID"] = studentId;
                                    temprow["STUDENT_NAME"] = "";
                                    temprow[teacherNameFieldName] = unkTeacherName;
                                    temprow["PERIOD"] = "01";
                                    temprow["LOCAL_COURSE_ID"] = "00000";
                                    temprow["SCHOOL2"] = campus;
                                    temprow["TEST_ID"] = benchmark;
                                    returnTable.Rows.Add(temprow);
                                }
                            }

                            else
                            {
                                // no rows found when removing BENCHMARK_MOD criteria, put "UNKNOWN"
                                DataRow temprow = returnTable.NewRow();
                                temprow["LOCAL_STUDENT_ID"] = studentId;
                                temprow["STUDENT_NAME"] = "";
                                temprow[teacherNameFieldName] = unkTeacherName;
                                temprow["PERIOD"] = "01";
                                temprow["LOCAL_COURSE_ID"] = "00000";
                                temprow["SCOHOL_ABBR"] = campus;
                                temprow["TEST_ID"] = benchmark;
                                returnTable.Rows.Add(temprow);
                            }
                        }
                    }
                }
            }

            dsReturn.Tables.Add(returnTable);

            return dsReturn;
        }



        public static DataSet makeUniqueDataSet(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                // get column names of the table in this dataset
                List<string> colnames = new List<string>();
                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    colnames.Add(col.ColumnName);
                }
                string[] colnamesstr = colnames.ToArray();

                // create a new table with uniqueness
                DataTable uniqueTable = ds.Tables[0].DefaultView.ToTable(true, colnamesstr);

                // return a dataset with this new table
                DataSet returnDS = new DataSet();
                returnDS.Tables.Add(uniqueTable);
                return returnDS;
            }

            return ds;
        }




        public static string GetRawCustomQuery(string test)
        {
            string getCustomQueryQuery =
            "select custom_query from aci.test_definition where test_id = '" +
            test + "'";
            DataSet ds = dbIFOracle.getDataRows(getCustomQueryQuery);
            string query = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            if (demoMode)
            {
                query = query.Replace(dbStudentRoster_reg, dbStudentRoster_temp);
            }

            return query;
        }


        //**********************************************************************//
        //** returns a DataSet with the results of the main "custom query" 
        //** defined for the specified test at the specified campus
        //**
        public static DataSet executeStudentListQuery(string benchmark, string campus)
        {
            // get the CUSTOM_QUERY defined in the database
            string rawCustomQuery = GetRawCustomQuery(benchmark);

            // put the correct school in the query
            string curSchoolAbbrevList = convertCampusList(campus);
            string tempschoolCustomQuery = rawCustomQuery.Replace(" = @school", " in (" + curSchoolAbbrevList + ")");
            string schoolCustomQuery = tempschoolCustomQuery.Replace("\n", " ");

            // change the teacher name-number part: we only need the name
            schoolCustomQuery = schoolCustomQuery.Replace(teacherNameNumFieldName, teacherNameFieldNameR);

            // run the query
            DataSet workingds = dbIFOracle.getDataRows(schoolCustomQuery);

            return (workingds);
        }


        //**********************************************************************//
        //** grade a scanned-in test and provide detailed restuls - compare the 
        //** list of answers on a scanned-in sheet to the test's answer key;
        //** return a DataSet with a table that consists of:
        //** ITEM_NUM (int), CORRECT (bool), CORRECT_ANS, STUDENT_ANS
        //**
        public static DataTable gradeScannedTestDetail(string testID, string studentAnswerString, string curCampus, int ansKeyIncAmt, string teacher, string period)
        {
            DataSet dsAnswerKey = new DataSet();
            DataView dvAnswerKey = new DataView();
            string[] columnLabels = { "ITEM_NUM", "CORRECT", "CORRECT_ANS", "STUDENT_ANS", "OBJECTIVE", "TEKS" };
            int itemObjective = new int();
            int curItemNum = new int();
            //int thisTestTemplType;

            //get the test template type for use in calculating griddables
            //thisTestTemplType = getTestTemplateType(testID);

            //convert answer string to an array
            string[] studentAnswerStringArray = studentAnswerString.Split(',');

            // do special processing for special template types
            if (GridHandler.isGriddable(testID))
                GridHandler.ProcessAnswerStringWithGrids(studentAnswerStringArray, testID, curCampus);

            if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(testID))
                MultiAnswerTemplateHandler.ProcessAnswerStringWithMultiAnswers(studentAnswerStringArray, testID, curCampus);

            //get the answer key as a DataSet
            dsAnswerKey = getTestAnswerKey(testID, curCampus, ansKeyIncAmt, teacher, period);
            dvAnswerKey = new DataView(dsAnswerKey.Tables[0]);


            DataTable table = new DataTable();
            table.Columns.Add(columnLabels[0], System.Type.GetType("System.Int32"));
            table.Columns.Add(columnLabels[1], System.Type.GetType("System.Boolean"));
            table.Columns.Add(columnLabels[2], System.Type.GetType("System.String"));
            table.Columns.Add(columnLabels[3], System.Type.GetType("System.String"));
            table.Columns.Add(columnLabels[4], System.Type.GetType("System.Int32"));
            table.Columns.Add(columnLabels[5], System.Type.GetType("System.String"));

            //grade each item on the test
            int numTotal = dvAnswerKey.Table.Rows.Count;
            for (int i = 0; i < numTotal; i++)
            {
                string correctAnswer = dvAnswerKey.Table.Rows[i]["ANSWER"].ToString();
                itemObjective = Convert.ToInt32(dvAnswerKey.Table.Rows[i]["OBJECTIVE"].ToString());
                string itemTEKS = dvAnswerKey.Table.Rows[i]["TEKS"].ToString();
                curItemNum = (int)(decimal)dvAnswerKey.Table.Rows[i]["ITEM_NUM"];

                DataRow row = table.NewRow();
                row[columnLabels[0]] = curItemNum;                      // ITEM_NUM
                row[columnLabels[2]] = correctAnswer;                   // write the correct answer
                row[columnLabels[4]] = itemObjective;                   // the Objective number
                row[columnLabels[5]] = itemTEKS;                        // the TEKS code


                if (studentAnswerStringArray.Length >= curItemNum)
                {                                                       // a student answer exists
                    string studentAnswer = studentAnswerStringArray[curItemNum - 1];
                    studentAnswer = (studentAnswer.Trim() == "") ? "-" : studentAnswer;
                    row[columnLabels[3]] = studentAnswer;
                    if (studentAnswer == correctAnswer)
                    {                                                   // correct answer
                        row[columnLabels[1]] = true;
                    }
                    else if (studentAnswer == ExceptionHandler.getAlternateAnswer(testID, curItemNum, curCampus))
                    {
                        row[columnLabels[1]] = true;
                    }
                    else
                    {                                                   // incorrect answer
                        row[columnLabels[1]] = false;
                    }
                }
                else
                {                                                       // no student answer
                    row[columnLabels[3]] = "";
                    row[columnLabels[1]] = false;
                }
                table.Rows.Add(row);
            }

            return table;

        }


        //**********************************************************************//
        //** grade a scanned-in test - compare the list of answers on a 
        //** scanned-in sheet to the test's answer key;
        //** return a DataSet with a table that consists of:
        //** LETTER_GRADE, NUM_CORRECT, NUM_TOTAL, PCT_CORRECT, PASS_NUM, 
        //** COMMENDED_NUM
        //**
        public static DataTable gradeScannedTest(string testID, string studentAnswerString, string curCampus, int ansKeyIncAmt, string teacher, string period)
        {
            DataSet dsAnswerKey = new DataSet();
            DataView dvAnswerKey = new DataView();
            int numCorrect, numTotal = new int();
            int curItemNum = new int();
            decimal pctCorrect = new decimal();
            char letterGrade = new char();
            string[] columnLabels = { "LETTER_GRADE", "NUM_CORRECT", "NUM_TOTAL", "PCT_CORRECT", 
                                      "PASS_NUM", "COMMENDED_NUM" };
            //int thisTestTemplType;

            //get pass and commended numbers for this test
            int passNum = getTestPassingNum(testID);
            int commendedNum = getTestCommendedNum(testID);

            //get the test template type for use in calculating griddables
            //thisTestTemplType = getTestTemplateType(testID);

            //convert answer string to an array
            string[] studentAnswerStringArray = studentAnswerString.Split(',');

            // do special processing for special template types
            if (GridHandler.isGriddable(testID))
                GridHandler.ProcessAnswerStringWithGrids(studentAnswerStringArray, testID, curCampus);

            if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(testID))
                MultiAnswerTemplateHandler.ProcessAnswerStringWithMultiAnswers(studentAnswerStringArray, testID, curCampus);

            //get the answer key as a DataSet
            dsAnswerKey = getTestAnswerKey(testID, curCampus, ansKeyIncAmt, teacher, period);
            dvAnswerKey = new DataView(dsAnswerKey.Tables[0]);

            //grade each item on the test
            numCorrect = 0;
            numTotal = dvAnswerKey.Table.Rows.Count;

            for (int i = 0; i < numTotal; i++)
            {
                curItemNum = (int)(decimal)dvAnswerKey.Table.Rows[i]["ITEM_NUM"];


                if (studentAnswerStringArray.Length >= curItemNum)       // a student answer exists
                {
                    if (studentAnswerStringArray[curItemNum - 1] == dvAnswerKey.Table.Rows[i]["ANSWER"].ToString())
                    {
                        // student's answer is correct
                        numCorrect++;
                    }
                    if (studentAnswerStringArray[curItemNum - 1] == ExceptionHandler.getAlternateAnswer(testID, curItemNum, curCampus))
                    {
                        // student's answer is correct as the alternate answer
                        numCorrect++;
                    }
                }
            }

            //calculate stuff
            pctCorrect = (decimal)numCorrect / (decimal)numTotal;
            letterGrade = birUtilities.calcLetterGrade(testID, numCorrect, numTotal, passNum, commendedNum);

            //return the results
            DataTable table = new DataTable();
            table.Columns.Add(columnLabels[0], System.Type.GetType("System.Char"));
            table.Columns.Add(columnLabels[1], System.Type.GetType("System.Int32"));
            table.Columns.Add(columnLabels[2], System.Type.GetType("System.Int32"));
            table.Columns.Add(columnLabels[3], System.Type.GetType("System.Decimal"));
            table.Columns.Add(columnLabels[4], System.Type.GetType("System.Int32"));
            table.Columns.Add(columnLabels[5], System.Type.GetType("System.Int32"));
            DataRow row = table.NewRow();
            row[columnLabels[0]] = letterGrade;                 // letter grade
            row[columnLabels[1]] = numCorrect;                  // num correct
            row[columnLabels[2]] = numTotal;                    // num total
            row[columnLabels[3]] = pctCorrect;                  // pct correct
            row[columnLabels[4]] = passNum;                     // pass num
            row[columnLabels[5]] = commendedNum;                // commended num
            table.Rows.Add(row);

            return table;
        }



        //**********************************************************************//
        //** return a DataSet with the answer keys for the specified test
        //** and campus
        //**
        public static DataSet getTestAnswerKey(string testID, string campus, int ansKeyIncAmt, string teacher, string period)
        {
            DataSet dsBothAnsKeysUnsorted = getDistrictAnswerKey(testID, campus);
            DataSet dsCampusAnsKey = getCampusAnswerKey(testID, campus, ansKeyIncAmt, teacher, period);
            DataSet dsBothAnsKeys = new DataSet();

            dsBothAnsKeysUnsorted.Tables[0].Merge(dsCampusAnsKey.Tables[0], true, MissingSchemaAction.Ignore);
            DataTable dtSorted = sortTableByColumn(dsBothAnsKeysUnsorted.Tables[0], "ITEM_NUM");

            dsBothAnsKeys.Tables.Add(dtSorted);

            // convert answer key items if necessary
            if (TFTemplateHandler.IsTFTemplate(testID))
                dsBothAnsKeys = TFTemplateHandler.ProcessAnswerKeyWithTF(dsBothAnsKeys, testID);
            if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(testID))
                dsBothAnsKeys = MultiAnswerTemplateHandler.ProcessAnswerKeyWithMultiAnswers(dsBothAnsKeys, testID);

            //// check for any items that need to be dropped from the test
            //int[] distDropList = ExceptionHandler.getItemDropList(testID, campus);
            //if (distDropList != null)
            //{
            //    // remove the dropped items from the results
            //    int curItemNum = new int();
            //    for (int rowIdx = 0; rowIdx < dsBothAnsKeys.Tables[0].Rows.Count; rowIdx++)
            //    {
            //        // if this row's item number is in the drop list, then delete it
            //        curItemNum = (int)(decimal)dsBothAnsKeys.Tables[0].Rows[rowIdx]["ITEM_NUM"];
            //        for (int j = 0; j < distDropList.Length; j++)
            //        {
            //            if (curItemNum == distDropList[j])
            //            {
            //                dsBothAnsKeys.Tables[0].Rows[rowIdx].Delete();
            //                break;
            //            }
            //        }
            //    }
            //}

            dsBothAnsKeys.Tables[0].AcceptChanges();
            return dsBothAnsKeys;
        }

        private static DataTable sortTableByColumn(DataTable dt, string sortCol)
        {
            DataTable dtSorted = dt.Clone();

            DataRow[] rowsSorted = dt.Select("", sortCol, DataViewRowState.CurrentRows);
            foreach (DataRow row in rowsSorted)
            {
                DataRow newrow = dtSorted.NewRow();
                newrow.ItemArray = row.ItemArray;
                dtSorted.Rows.Add(newrow);
            }

            return dtSorted;
        }


        //**********************************************************************//
        //** return a DataSet with the District (common) portion of the 
        //** answer key for the specified test
        //**
        public static DataSet getDistrictAnswerKey(string testID, string campus)
        {
            string qs = getDistrictTestAnswerKeyQuery.Replace("@testId", testID);
            DataSet dsDistrictAnsKey = dbIFOracle.getDataRows(qs);

            // check for any items that need to be dropped from the test
            int[] distDropList = ExceptionHandler.getItemDropList(testID, campus);
            if (distDropList != null)
            {
                // remove the dropped items from the results
                int curItemNum = new int();
                for (int rowIdx = 0; rowIdx < dsDistrictAnsKey.Tables[0].Rows.Count; rowIdx++)
                {
                    // if this row's item number is in the drop list, then delete it
                    curItemNum = (int)(decimal)dsDistrictAnsKey.Tables[0].Rows[rowIdx]["ITEM_NUM"];
                    for (int j = 0; j < distDropList.Length; j++)
                    {
                        if (curItemNum == distDropList[j])
                        {
                            dsDistrictAnsKey.Tables[0].Rows[rowIdx].Delete();
                            break;
                        }
                    }
                }
            }

            // my stupid, hard-coded changes

            // for Biology Unit 2 CUT, regular and PreAP tests item 3, answer = B for RHS ONLY
            if ((testID == "2011-09 SC Biology Unit 2 CUT 99-21" || testID == "2011-09 SC Biology Unit 2 PreAP CUT 99-23") &&
                campus == "RHS")
            {
                dsDistrictAnsKey.Tables[0].Rows[2]["ANSWER"] = "B";
            }

            dsDistrictAnsKey.Tables[0].AcceptChanges();
            return dsDistrictAnsKey;
        }


        //**********************************************************************//
        //** return a DataSet with the portion of the answer key for the 
        //** specified test and campus if there is one; will return an empty
        //** DataSet if there is no campus-specific answer key for this test
        //**
        public static DataSet getCampusAnswerKey(string testID, string campus, int ansKeyIncAmt, string teacher, string period)
        {
            // try this
            ansKeyIncAmt = (ansKeyIncAmt == 0) ? birIF.maxNumTestQuestions : ansKeyIncAmt;

            int minItemNum = 1 + ansKeyIncAmt;
            int maxItemNum = ansKeyIncAmt + birIF.maxNumTestQuestions;
            int readItemNum = new int();
            string lblItemNum = "ITEM_NUM";

            string qs = getCampusTestAnswerKeyQuery.Replace("@testId", testID);
            qs = qs.Replace("@schoolAbbr", campus);
            qs = qs.Replace("@itemNumStart", minItemNum.ToString());
            qs = qs.Replace("@itemNumEnd", maxItemNum.ToString());
            DataSet dsCampusAnsKey = dbIFOracle.getDataRows(qs);

            // adjust the item numbers if necessary


            if (ansKeyIncAmt > 0)
            {
                DataSet dsCampusAnsKeyModified = new DataSet();
                for (int i = 0; i < dsCampusAnsKey.Tables[0].Rows.Count; i++)
                {
                    readItemNum = (int)(decimal)dsCampusAnsKey.Tables[0].Rows[i][lblItemNum];
                    dsCampusAnsKey.Tables[0].Rows[i][lblItemNum] = readItemNum - ansKeyIncAmt;
                }
            }

            // check for any items that need to be dropped from the test
            int[] campusdroplist = ExceptionHandler.getItemDropList(testID, campus, teacher, period);
            if (campusdroplist != null)
            {
                // remove the dropped items from the results
                int curitemnum = new int();
                for (int rowidx = 0; rowidx < dsCampusAnsKey.Tables[0].Rows.Count; rowidx++)
                {
                    // if this row's item number is in the drop list, then delete it
                    curitemnum = (int)(decimal)dsCampusAnsKey.Tables[0].Rows[rowidx][lblItemNum];
                    for (int j = 0; j < campusdroplist.Length; j++)
                    {
                        if (curitemnum == campusdroplist[j])
                        {
                            dsCampusAnsKey.Tables[0].Rows[rowidx].Delete();
                            break;
                        }
                    }
                }
            }

            dsCampusAnsKey.Tables[0].AcceptChanges();
            return dsCampusAnsKey;
        }




        //**********************************************************************//
        //** find the row of the BENCHMARK data table that has the most recent
        //** scan for the specified studentId and testId
        //** returns NULL if no row found
        //**
        public static DataRow getLatestScanDataRow(string studentId, string testId)
        {
            string q = birIF.latestScanQuery;

            q = q.Replace("@studentId", studentId);
            q = q.Replace("@testId", testId);

            DataSet ds = dbIFOracle.getDataRows(q);

            if (ds.Tables[0].Rows.Count != 0)
            {
                return ds.Tables[0].Rows[0];
            }
            else
            {
                return null;
            }
        }


        //**********************************************************************//
        //** just like getLatestScanDataRow, but uses an existing data table
        //** rather than the Oracle database - saves time baby
        //**
        public static DataRow getLatestScanDataRowFromDataTable(string studentId, string testId, DataTable theTable)
        {
            string selectString =
                "TEST_ID = \'" + testId + "\' " +
                "AND STUDENT_ID = \'" + studentId + "\'";

            DataView tempDv = new DataView(theTable, selectString, "STUDENT_ID ASC", DataViewRowState.CurrentRows);
            DataTable curTable = tempDv.ToTable();
            DataRow curLatestRow = curTable.NewRow();

            // find the latest one for this student & test
            DateTime mostRecentDateTime = new DateTime(2000, 1, 1, 0, 0, 0);
            for (int i = 0; i < curTable.Rows.Count; i++)
            {
                DateTime thisRowDateTime = DateTime.Parse(curTable.Rows[i]["DATE_SCANNED"].ToString());
                if (thisRowDateTime > mostRecentDateTime)
                {
                    curLatestRow.ItemArray = curTable.Rows[i].ItemArray;
                    mostRecentDateTime = thisRowDateTime;
                }
            }


            return curLatestRow;
        }


        //**********************************************************************//
        //** return a dataset with the collection of the most recent rows
        //** from the BENCHMARK table for the list of student ids and test ids
        //** specified
        //**
        public static DataSet getLatestScanDataRows(string[] studentIds, string[] testIds)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add();
            setupTableLikeTable(ds.Tables[0], dbScans);

            string qs = birIF.getStudentScansQuery.Replace("@testIdList", birUtilities.convertStringArrayForQuery(testIds));

            DataSet scanTableMatches = dbIFOracle.getDataRows(qs);


            for (int i = 0; i < studentIds.Length; i++)
            {
                for (int j = 0; j < testIds.Length; j++)
                {
                    DataRow latestRow = getLatestScanDataRowFromDataTable(studentIds[i], testIds[j], scanTableMatches.Tables[0]);
                    if (latestRow != null)
                    {
                        DataRow tempRow = ds.Tables[0].NewRow();
                        tempRow.ItemArray = latestRow.ItemArray;
                        ds.Tables[0].Rows.Add(tempRow);
                    }
                }
                if (i % 1000 == 0)
                    Console.WriteLine("h");
            }

            return ds;
        }


        //**********************************************************************//
        //** sets up a data table with the same columns as the specified table
        //**
        public static void setupTableLikeTable(DataTable theTable, string theTableName)
        {

            DataTable schemaInfoTable = dbIFOracle.getTableSchemaInfo(theTableName);

            foreach (DataRow curColInfo in schemaInfoTable.Rows)
            {
                theTable.Columns.Add(curColInfo["ColumnName"].ToString(), (Type)curColInfo["DataType"]);
            }

            return;
        }


        //**********************************************************************//
        //** returns the passing number for the specified test
        //**
        public static int getTestPassingNum(string theTest)
        {
            string qs = queryGetPassNum.Replace("@testId", theTest);
            DataSet ds = dbIFOracle.getDataRows(qs);

            decimal nd = (decimal)ds.Tables[0].Rows[0][0];
            return (int)nd;
        }


        //**********************************************************************//
        //** returns the commended number for the specified test
        //**
        public static int getTestCommendedNum(string theTest)
        {
            string qs = queryGetCommendedNum.Replace("@testId", theTest);
            DataSet ds = dbIFOracle.getDataRows(qs);

            decimal nd = (decimal)ds.Tables[0].Rows[0][0];
            return (int)nd;
        }


        ////**********************************************************************//
        ////** returns a string array consisting of {"<teacher>","<period>"}
        ////** for a given student and test; i.e. this gets the student's
        ////** teacher and period for the test
        //public static string[] getTeacherPeriodForStudentInCourses(string studentID, string[] courses)
        //{
        //    // add a leading 0 to student ID if needed
        //    if (studentID.Length == 5)
        //        studentID = "0" + studentID;

        //    string qs = queryGetStudentRosterDataForTest.Replace("@studentID", studentID);
        //    qs = qs.Replace("@courses", birUtilities.convertStringArrayForQuery(courses));

        //    DataSet ds = dbIFOracle.getDataRows(qs);

        //    // very temporary workaround
        //    if (studentID == "120542")
        //    {
        //        return new string[] { "KLINE, REBECCA", "02" };
        //    }
        //    // end workaround

        //    if (ds.Tables[0].Rows.Count == 0)
        //        return new string[] { "UNKNOWN", "01" };

        //    string[] strTeacherPeriod = {ds.Tables[0].Rows[0][teacherNameFieldName].ToString(),
        //                   ds.Tables[0].Rows[0]["PERIOD"].ToString()};

        //    return strTeacherPeriod;
        //}


        public static bool isTestElementary(string testId)
        {
            string qs = testTypeQuery.Replace("@testId", testId);
            DataSet ds = dbIFOracle.getDataRows(qs);

            if (ds.Tables[0].Rows[0][0].ToString() == "E")
                return true;

            return false;
        }

        public static string[] getElemAbbrList()
        {
            string qs = getELCampusQuery;
            DataSet ds = dbIFOracle.getDataRows(qs);

            List<string> abbrList = new List<string>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                abbrList.Add(row[0].ToString());
            }

            return abbrList.ToArray<string>();
        }


        public static string[] getSecAbbrList()
        {
            string qs1 = getJHCampusQuery;
            string qs2 = getHSCampusQuery;
            DataSet ds1 = dbIFOracle.getDataRows(qs1);
            DataSet ds2 = dbIFOracle.getDataRows(qs2);

            ds1.Merge(ds2);

            List<string> abbrList = new List<string>();

            foreach (DataRow row in ds1.Tables[0].Rows)
            {
                abbrList.Add(row[0].ToString());
            }

            return abbrList.ToArray<string>();
        }



        internal static string convertCampusList(string campus)
        {
            // if "ALL ELEMENTARY", get a list of all elementaries
            if (campus == "ALL Elementary")
            {
                return birUtilities.convertStringArrayForQuery(getElemAbbrList());
            }

            // if "ALL SECONDARY", get a list of all secondary schools
            else if (campus == "ALL Secondary")
            {
                return birUtilities.convertStringArrayForQuery(getSecAbbrList());
            }

            // just return the single campus abbreviation
            else
            {
                return "\'" + campus + "\'";
            }
        }

        internal static bool isStudentElemNoAttCourse(string studentId)
        {
            string qs = elemStudentNotInAttCourseQuery.Replace("@studentId", studentId);
            DataSet ds = dbIFOracle.getDataRows(qs);

            if (ds.Tables.Count > 0)
                return true;

            return false;
        }

        public static string[] getTestListForSchool(string school)
        {
            DataSet ds = new DataSet();

            string theSchoolType = getSchoolType(school);

            if (theSchoolType == "A")
                ds = dbIFOracle.getDataRows(queryGetTestListAllTests);
            else if (theSchoolType == "S")
            {
                string qs = queryGetTestListBySchoolTypeSec.Replace("@schoolType", "J','H");
                ds = dbIFOracle.getDataRows(qs);
            }
            else if (theSchoolType == "J" || theSchoolType == "H")
            {
                string qs = queryGetTestListBySchoolTypeSec.Replace("@schoolType", theSchoolType);
                ds = dbIFOracle.getDataRows(qs);
            }
            else
            {
                string qs = queryGetTestListBySchoolType.Replace("@schoolType", theSchoolType);
                ds = dbIFOracle.getDataRows(qs);
            }

            List<string> testList = new List<string>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                testList.Add(ds.Tables[0].Rows[i]["TEST_ID"].ToString());
            }

            return testList.ToArray();
        }

        /// <summary>
        /// returns a one character string indicating the school type
        /// </summary>
        /// <param name="school">school abbreviation code</param>
        /// <returns>A = both Elem & Sec; E = Elementary; S = Secondary; J = Sec. Jr. High; H = Sec. High School</returns>
        public static string getSchoolType(string school)
        {
            if (school == "ALL Elementary")
                return "E";
            else if (school == "ALL Secondary")
                return "S";
            else if (school == "ALL")
                return "A";

            string qs = queryGetSchoolID.Replace("@schoolAbbr", school);
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return "A";
            decimal nd = (decimal)ds.Tables[0].Rows[0][0];
            int sid = (int)nd;

            if (sid == 6)                   // CMLC = All
                return "A";
            if (sid < 10 || sid == 52)      // High School
                return "H";
            if (sid >= 10 && sid < 100)     // Jr High
                return "J";
            if (sid >= 100)                 // Elementrary
                return "E";

            return "A";
        }


    }





}