using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;




//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
// an interface class for the Student Statistics report
//
// matt hollingsworth
// oct 2010
//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *



namespace Benchmark_Instant_Reports_2
{
    public class StudentStatsIF
    {

        public static string studentStatsResultsDatatableName = "ACI.TEMP_RESULTS_STUDENTSTATS";



        //**********************************************************************//
        //** generate a DataTable to be used for the Student Statistics report
        //** for the specified Campus, Benchmark, Teacher and Period
        //** via the DataTable passed that contains the current student
        //** records already filtered.
        //**
        //** DataSet.Table[0].Columns :
        //** (student_id, student_name, test_id, scan_datetime, letter_grade,
        //**    num_correct, num_total, pct_correct)
        public static DataTable generateStudentStatsRepTable(DataTable studentData, string curTest, string curCampus)
        {
            DataView dv = new DataView(studentData);
            string curId;
            DataSet dsFinal = new DataSet();
            DataTable table = new DataTable();
            int ansKeyVersionIncrement = new int();
            string lblStudentID = "STUDENT_ID";
            string lblStudentName = "STUDENT_NAME";
            string lblTestId = "TEST_ID";
            string lblScanDate = "SCAN_DATETIME";
            string lblCampus = "CAMPUS";
            string lblTeacher = "TEACHER";
            string lblPeriod = "PERIOD";
            string lblLetterGrade = "LETTER_GRADE";
            string lblNumCorrect = "NUM_CORRECT";
            string lblNumTotal = "NUM_TOTAL";
            string lblPctCorrect = "PCT_CORRECT";
            string lblPassNum = "PASS_NUM";
            string lblCommendedNum = "COMMENDED_NUM";

            // create columns for the resulting table
            table.Columns.Add(new DataColumn(lblStudentID, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblStudentName, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblTestId, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblScanDate, System.Type.GetType("System.DateTime")));
            table.Columns.Add(new DataColumn(lblLetterGrade, System.Type.GetType("System.Char")));
            table.Columns.Add(new DataColumn(lblNumCorrect, System.Type.GetType("System.Int32")));
            table.Columns.Add(new DataColumn(lblNumTotal, System.Type.GetType("System.Int32")));
            table.Columns.Add(new DataColumn(lblPctCorrect, System.Type.GetType("System.Decimal")));
            table.Columns.Add(new DataColumn(lblPassNum, System.Type.GetType("System.Int32")));
            table.Columns.Add(new DataColumn(lblCommendedNum, System.Type.GetType("System.Int32")));
            table.Columns.Add(new DataColumn(lblCampus, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblTeacher, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblPeriod, System.Type.GetType("System.String")));


            // grade each student's test and add it to the DataSet
            int numNull = 0;
            for (int j = 0; j < dv.Table.Rows.Count; j++)
            {
                curId = dv.Table.Rows[j]["local_student_id"].ToString();
                DataRow curScanDataRow = birIF.getLatestScanDataRow(curId, curTest);
                if (curScanDataRow != null)
                {
                    DataRow thisrow = table.NewRow();
                    thisrow[lblStudentID] = curId;
                    string curName = birUtilities.lookupStudentName(curId);
                    thisrow[lblStudentName] = curName;
                    thisrow[lblTestId] = curTest;
                    thisrow[lblScanDate] = DateTime.Parse(curScanDataRow["DATE_SCANNED"].ToString());
                    thisrow[lblCampus] = curCampus;
                    thisrow[lblTeacher] = dv.Table.Rows[j][birIF.teacherNameFieldName].ToString();
                    thisrow[lblPeriod] = dv.Table.Rows[j]["PERIOD"].ToString();

                    ansKeyVersionIncrement = birExceptions.campusAnswerKeyVersionIncrement(curTest, curCampus, 
                        thisrow[lblTeacher].ToString(), thisrow[lblPeriod].ToString());
                    DataTable resultTable = birIF.gradeScannedTest(curTest, curScanDataRow["ANSWERS"].ToString(), curCampus, ansKeyVersionIncrement);
                    thisrow[lblLetterGrade] = resultTable.Rows[0][lblLetterGrade];
                    thisrow[lblNumCorrect] = resultTable.Rows[0][lblNumCorrect];
                    thisrow[lblNumTotal] = resultTable.Rows[0][lblNumTotal];
                    thisrow[lblPctCorrect] = resultTable.Rows[0][lblPctCorrect];
                    thisrow[lblPassNum] = resultTable.Rows[0][lblPassNum];
                    thisrow[lblCommendedNum] = resultTable.Rows[0][lblCommendedNum];
                    table.Rows.Add(thisrow);
                }
                else
                {
                    numNull++;
                }
            }

            return table;
        }




 

        //**********************************************************************//
        //** write the table of current results to the temporary results
        //** data table so that the report can use it
        //**
        public static int writeStudentStatsResultsToDb(DataTable resultsTable)
        {
            int result = new int();
            DataSet ds = new DataSet();
            string qs = "SELECT * FROM " + studentStatsResultsDatatableName;
            string findcriteria1 = "";
            string findcriteria2 = "";
            DateTime datetimeNew, datetimeExisting = new DateTime();

            // get connection to the database
            string connectionString = ConfigurationManager.ConnectionStrings[dbIFOracle.databaseName].ConnectionString;
            using (OracleConnection connection =
                new OracleConnection(connectionString))
            {
                OracleDataAdapter adapter = new OracleDataAdapter();
                adapter.SelectCommand = new OracleCommand(qs, connection);
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                OracleCommandBuilder cmdb = new OracleCommandBuilder(adapter);
                adapter.Fill(ds, studentStatsResultsDatatableName);
                ds.Tables[0].Columns["GRADED_ANSWERS"].MaxLength = 1500;
                ds.Tables[0].Columns["GRADED_ANSWERS_FORMATTED"].MaxLength = 1500;


                // write the data to the DataSet
                foreach (DataRow row in resultsTable.Rows)
                {
                    // check and see if current record already exists and is the latest one
                    findcriteria1 = "TEST_ID = \'" + row["TEST_ID"].ToString() + "\' " +
                                    "AND STUDENT_ID = \'" + row["STUDENT_ID"].ToString() + "\' ";
                    findcriteria2 = "SCAN_DATETIME = \'" + row["SCAN_DATETIME"].ToString() + "\'";
                    int l = ds.Tables[0].Select(findcriteria1).Length;

                    if (l == 0)                 // test_id and student_id not there at all, add it to the dataset maybe
                    {

                        DataRow row2 = ds.Tables[0].NewRow();
                        row2.ItemArray = row.ItemArray;
                        ds.Tables[0].Rows.Add(row2);
                    }
                    else                        // test_id and student_id are there, compare scandatetimes 
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            if ((ds.Tables[0].Rows[i]["TEST_ID"].ToString() == row["TEST_ID"].ToString())
                                && (ds.Tables[0].Rows[i]["STUDENT_ID"].ToString() == row["STUDENT_ID"].ToString()))
                            {
                                datetimeExisting = (DateTime)ds.Tables[0].Rows[i]["SCAN_DATETIME"];
                                datetimeNew = (DateTime)row["SCAN_DATETIME"];
                                
                                // always write it, even if it's there - it might not have the graded ans strings
                                if (true)
                                //if (datetimeNew > datetimeExisting)     // new record is newer, delete old one
                                {
                                    ds.Tables[0].Rows[i].ItemArray = row.ItemArray;
                                }
                                // otherwise just keep the existing one and move on
                            }
                    }
                }
                adapter.Update(ds, "ACI.TEMP_RESULTS_STUDENTSTATS");
            }

            result = 1;

            return result;
        }


    }
}