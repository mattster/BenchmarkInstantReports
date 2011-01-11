using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;



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
        #endregion

        #region birdbnames
        public static string dbScans = "ACI.BENCHMARK";
        public static string dbTestDefn = "ACI.TEST_DEFINITION";
        public static string dbSchool = "ACI.SCHOOL";
        public static string dbAnswerKey = "ACI.ANSWER_KEY";
        public static string dbAnswerKeyCampus = "ACI.ANSWER_KEY_CAMPUS";
        public static string dbObjectives = "ACI.OBJECTIVES";
        public static string dbStudentRoster = "SIS_ODS.RISD_STUDENT_ROSTER_VW";
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
        public static int answerKeyFillerObjective = 99;
        public static string answerKeyFillerTEKS = "ZZZZZ";

        public static string schoolCriteriaInCustomQ = "AND R.SCHOOL_ABBR = @school";

        private static string getHSCampusQuery = 
                "select school_abbr, schoolname from aci.school " +
                "where schoolid between 1 and 9 order by schoolname";
        
        private static string getJHCampusQuery =
                "select school_abbr, schoolname from aci.school " +
                "where schoolid between 10 and 99 order by schoolname";
        
        private static string getELCampusQuery =
                "select school_abbr, schoolname from aci.school " +
                "where schoolid between 10 and 999 order by schoolname";
        
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
        
        public static string getBenchmarkListQuery =
                "select test_id from " + dbTestDefn + " " +
                "where current_date <= add_months(to_date(end_datetime, 'MM/DD/YYYY'),1) " +
                "order by test_id asc";

        public static string latestScanQuery = "select * from (select * from aci.benchmark " +
                "where student_id = \'@studentId\' and test_id = \'@testId\' " +
                "order by to_date(date_scanned, 'MM/DD/YYYY HH:MI:SS AM') DESC ) " +
                "where rownum = 1";

        public static string numTestQuestionsQuery = "select * from ( " +
                "select item_num from aci.answer_key " +
                "where test_id = \'@testId\' order by item_num desc ) " +
                "where rownum = 1";

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
				"where test_id in (@testIdList) " +
                "and school_abbr = \'@campus\' " +
				"order by student_id asc";

        public static string getCustQueryQuery =
                "select custom_query " +
                "from " + dbTestDefn + " " +
                "where test_id = \'@testId\'";

        public static string queryGetStudentScansFromCampusCourse =
                "select local_student_id, student_name, " + 
                teacherNameFieldNameR + ", " +
                "period, local_course_id, school_abbr, test_id " +
                "from " + dbScans + " " +
                "join " + dbStudentRoster + " r " +
                "on student_id = local_student_id " +
                "where test_id = \'@testId\' " +
                "and school_abbr = \'@campus\' " +
                "and local_course_id in (@courses) " +
                "order by student_id asc";

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

#endregion


        //**********************************************************************//
        //** returns an array of course id's that are applicable to
        //** the given test
        public static string[] getCourseIDsForTest(string testId)
        {
            string custQueryQs = getCustQueryQuery.Replace("@testId", testId);

            DataSet dsGetQuery = dbIFOracle.getDataRows(custQueryQs);
            string modifiedCustomQuery = dsGetQuery.Tables[0].Rows[0][0].ToString().Replace(
                schoolCriteriaInCustomQ, "");

            DataSet dsCustomQuery = dbIFOracle.getDataRows(modifiedCustomQuery);

            return birUtilities.getUniqueTableColumnStringValues(dsCustomQuery.Tables[0], "LOCAL_COURSE_ID");
        }


        //**********************************************************************//
        //** returns a string array of list of student ID's that have 
        //** scanned answer sheets in the benchmark table for the given
        //** test_id and campus
        //**
        public static string[] getListOfStudentsWithScans(string[] benchmarks, string campus)
        {
            string qs = getStudentScansFromCampusQuery.Replace("@campus", campus);
            qs = qs.Replace("@testIdList", birUtilities.convertStringArrayForQuery(benchmarks));

            DataSet ds = dbIFOracle.getDataRows(qs);

            return birUtilities.getUniqueTableColumnStringValues(ds.Tables[0], "STUDENT_ID");
        }


        //**********************************************************************//
        //** returns a DataSet with the list of students from the selected
        //** campus that have scans for the specified test; data includes some
        //** matching data from the student roster
        //**
        public static DataSet getStudentScanListData(string benchmark, string campus)
        {
            string qs = queryGetStudentScansFromCampusCourse.Replace("@testId", benchmark);
            qs = qs.Replace("@campus", campus);
            qs = qs.Replace("@courses", birUtilities.convertStringArrayForQuery(
                getCourseIDsForTest(benchmark)));

            return dbIFOracle.getDataRows(qs);
        }


        //**********************************************************************//
        //** returns a DataSet with the results of the main "custom query" 
        //** defined for the specified test at the specified campus
        //**
        public static DataSet executeStudentListQuery(string benchmark, string campus)
        {
            // get the CUSTOM_QUERY defined in the database
            string getCustomQueryQuery =
                "select custom_query from aci.test_definition where test_id = '" +
                benchmark + "'";
            DataSet ds = dbIFOracle.getDataRows(getCustomQueryQuery);
            string rawCustomQuery = ds.Tables[0].Rows[0].ItemArray[0].ToString();

            // put the correct school in the query
            string curSchoolAbbrev = "\'" + campus + "\'";
            string tempschoolCustomQuery = rawCustomQuery.Replace("@school", curSchoolAbbrev);
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
        public static DataTable gradeScannedTestDetail(string testID, string studentAnswerString, string curCampus, int ansKeyIncAmt)
        {
            DataSet dsAnswerKey = new DataSet();
            DataView dvAnswerKey = new DataView();
            string[] columnLabels = { "ITEM_NUM", "CORRECT", "CORRECT_ANS", "STUDENT_ANS", "OBJECTIVE", "TEKS" };
            int itemObjective = new int();
            int curItemNum = new int();

            //convert answer string to an array
            string[] studentAnswerStringArray = studentAnswerString.Split(',');

            //get the answer key as a DataSet
            dsAnswerKey = getTestAnswerKey(testID, curCampus, ansKeyIncAmt);
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
                row[columnLabels[0]] = curItemNum;   // ITEM_NUM
                row[columnLabels[2]] = correctAnswer;                   // write the correct answer
                row[columnLabels[4]] = itemObjective;                   // the Objective number
                row[columnLabels[5]] = itemTEKS;                        // the TEKS code


                if (studentAnswerStringArray.Length >= curItemNum)       // a student answer exists
                {
                    string studentAnswer = studentAnswerStringArray[curItemNum - 1];
                    row[columnLabels[3]] = studentAnswer;
                    if (studentAnswer == correctAnswer)                 // correct answer
                    {
                        row[columnLabels[1]] = true;
                    }
                    else                                                // incorrect answer
                    {
                        row[columnLabels[1]] = false;
                    }
                }
                else                                                    // no student answer
                {
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
        public static DataTable gradeScannedTest(string testID, string studentAnswerString, string curCampus, int ansKeyIncAmt)
        {
            DataSet dsAnswerKey = new DataSet();
            DataView dvAnswerKey = new DataView();
            int numCorrect, numTotal = new int();
            int curItemNum = new int();
            decimal pctCorrect = new decimal();
            char letterGrade = new char();
            string[] columnLabels = { "LETTER_GRADE", "NUM_CORRECT", "NUM_TOTAL", "PCT_CORRECT", 
                                      "PASS_NUM", "COMMENDED_NUM" };
            
            //get pass and commended numbers for this test
            int passNum = getTestPassingNum(testID);
            int commendedNum = getTestCommendedNum(testID);

            //convert answer string to an array
            string[] studentAnswerStringArray = studentAnswerString.Split(',');

            //get the answer key as a DataSet
            dsAnswerKey = getTestAnswerKey(testID, curCampus, ansKeyIncAmt);
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
                }
            }
            
            //calculate stuff
            pctCorrect = (decimal)numCorrect / (decimal)numTotal;
            letterGrade = birUtilities.calcLetterGrade(testID, numCorrect, numTotal, false);

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
        public static DataSet getTestAnswerKey(string testID, string campus, int ansKeyIncAmt)
        {
            DataSet dsBothAnsKeys = getDistrictAnswerKey(testID, campus);
            DataSet dsCampusAnsKey = getCampusAnswerKey(testID, campus, ansKeyIncAmt);

            dsBothAnsKeys.Tables[0].Merge(dsCampusAnsKey.Tables[0], true, MissingSchemaAction.Ignore);

            return dsBothAnsKeys;
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
            int[] distDropList = birExceptions.getDistrictItemDropList(testID, campus);
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

            dsDistrictAnsKey.Tables[0].AcceptChanges();
            return dsDistrictAnsKey;
        }


        //**********************************************************************//
        //** return a DataSet with the portion of the answer key for the 
        //** specified test and campus if there is one; will return an empty
        //** DataSet if there is no campus-specific answer key for this test
        //**
        public static DataSet getCampusAnswerKey(string testID, string campus, int ansKeyIncAmt)
        {
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
            int[] campusDropList = birExceptions.getCampusItemDropList(testID, campus);
            if (campusDropList != null)
            {
                // remove the dropped items from the results
                int curItemNum = new int();
                for (int rowIdx = 0; rowIdx < dsCampusAnsKey.Tables[0].Rows.Count; rowIdx++)
                {
                    // if this row's item number is in the drop list, then delete it
                    curItemNum = (int)(decimal)dsCampusAnsKey.Tables[0].Rows[rowIdx][lblItemNum];
                    for (int j = 0; j < campusDropList.Length; j++)
                    {
                        if (curItemNum == campusDropList[j])
                        {
                            dsCampusAnsKey.Tables[0].Rows[rowIdx].Delete();
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


        //**********************************************************************//
        //** returns a string array consisting of {"<teacher>","<period>"}
        //** for a given student and test; i.e. this gets the student's
        //** teacher and period for the test
        public static string[] getTeacherPeriodForStudentInCourses(string studentID, string[] courses)
        {
            // add a leading 0 to student ID if needed
            if (studentID.Length == 5)
                studentID = "0" + studentID;
            
            string qs = queryGetStudentRosterDataForTest.Replace("@studentID", studentID);
            qs = qs.Replace("@courses", birUtilities.convertStringArrayForQuery(courses));

            DataSet ds = dbIFOracle.getDataRows(qs);

            string[] strTeacherPeriod = {ds.Tables[0].Rows[0][teacherNameFieldName].ToString(),
                           ds.Tables[0].Rows[0]["PERIOD"].ToString()};

            return strTeacherPeriod;
        }
    }
}