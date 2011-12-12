using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;


//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
// an interface class for the Benchmark Statistics report
//
// matt hollingsworth
// oct 2010
//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *



namespace Benchmark_Instant_Reports_2
{
    public class BenchmarkStatsIF
    {
        #region PublicStuff
        public static string benchmarkStatsResultsDatatableName = "ACI.TEMP_RESULTS_BENCHMARKSTATS";
        static string lblCampus = "CAMPUS";
        static string lblTestId = "TEST_ID";
        static string lblTeacher = "TEACHER";
        static string lblPeriod = "PERIOD";
        static string lblItemNum = "ITEM_NUM";
        static string lblPctCorrect = "PCT_CORRECT";
        static string lblNumCorrect = "NUM_CORRECT";
        static string lblNumTotal = "NUM_TOTAL";
        static string lblNumA = "NUM_A";
        static string lblNumB = "NUM_B";
        static string lblNumC = "NUM_C";
        static string lblNumD = "NUM_D";
        static string lblNumE = "NUM_E";
        static string lblNumF = "NUM_F";
        static string lblNumG = "NUM_G";
        static string lblNumH = "NUM_H";
        static string lblNumJ = "NUM_J";
        static string lblNumK = "NUM_K";
        static string lblAnswer = "ANSWER";
        static string lblObjective = "OBJECTIVE";
        static string lblTEKS = "TEKS";
        #endregion

        //*******************
        //* class to manage the counts for 
        //* the different answer choices for a test
        //********
         public class answerCounter
        {
            public int a, b, c, d, e, f, g, h, j, k;


            // default constructor
            public answerCounter()
            {
                a = 0;
                b = 0;
                c = 0;
                d = 0;
                e = 0;
                f = 0;
                g = 0;
                h = 0;
                j = 0;
                k = 0;
            }

            public void Increment(string x) 
            {
                if (x.ToUpper() == "A")
                    a++;
                else if (x.ToUpper() == "B")
                    b++;
                else if (x.ToUpper() == "C")
                    c++;
                else if (x.ToUpper() == "D")
                    d++;
                else if (x.ToUpper() == "E")
                    e++;
                else if (x.ToUpper() == "F")
                    f++;
                else if (x.ToUpper() == "G")
                    g++;
                else if (x.ToUpper() == "H")
                    h++;
                else if (x.ToUpper() == "J")
                    j++;
                else if (x.ToUpper() == "K")
                    k++;
                
                return; 
            }

            public void Reset()
            {
                a = 0;
                b = 0;
                c = 0;
                d = 0;
                e = 0;
                f = 0;
                g = 0;
                h = 0;
                j = 0;
                k = 0;

                return;
            }

        }


        

        //**********************************************************************//
        //** generate a DataTable to be used for the Benchmark Statistics report
        //** for the specified Campus and Benchmark 
        //** via the DataTable passed that contains the current student
        //** records already filtered.
        //**
        //** DataSet.Table[0].Columns :
        //** (campus, test_id, teacher, period, item_num, pct_correct,
        //**    num_correct, num_total)
        public static DataTable generateBenchmarkStatsRepTable(DataTable studentData, string curTest, string curCampus)
        {
            DataView dvStudentData = new DataView(studentData);
            string curId;
            DataSet dsFinal = new DataSet();
            DataTable resultsTable = new DataTable();
            int curNumCorrect, curNumTotal = new int();
            answerCounter curAnsCount = new answerCounter();

            setupResultsDataTable(resultsTable);

            // grade each student's test and add it to the DataSet
            int numNull = 0;
            for (int jj = 0; jj < dvStudentData.Table.Rows.Count; jj++)
            {
                curId = dvStudentData.Table.Rows[jj]["local_student_id"].ToString();
                
                DataRow curScanDataRow = birIF.getLatestScanDataRow(curId, curTest);
                if (curScanDataRow != null)
                {
                    DataTable gradedTable = birIF.gradeScannedTestDetail(curTest, curScanDataRow["ANSWERS"].ToString(), curCampus, 0,
                        dvStudentData.Table.Rows[jj][birIF.teacherNameFieldName].ToString(),
                        dvStudentData.Table.Rows[jj]["PERIOD"].ToString());
                    
                    // add data for each test item to the resultsTable
                    for (int k = 0; k < gradedTable.Rows.Count; k++)
                    {
                        // see if this item is already in the dataset
                        string selectString =
                            //"CAMPUS = \'" + dvStudentData.Table.Rows[j]["SCHOOL_ABBR"].ToString() + "\' and " +
                            "CAMPUS = \'" + dvStudentData.Table.Rows[jj]["SCHOOL2"].ToString() + "\' and " +
                            "TEST_ID = \'" + curTest + "\' and " +
                            "TEACHER = \'" + dvStudentData.Table.Rows[jj][birIF.teacherNameFieldName].ToString().Replace("'", "''") + "\' and " +
                            "PERIOD = \'" + dvStudentData.Table.Rows[jj]["PERIOD"].ToString() + "\' and " +
                            "ITEM_NUM = " + gradedTable.Rows[k]["ITEM_NUM"];
                        DataRow[] selectedRows = resultsTable.Select(selectString);

                        if (selectedRows.Length > 0)
                        {
                            // this item is here - get the values and then delete it
                            curNumCorrect = (int)selectedRows[0][lblNumCorrect];
                            curNumTotal = (int)selectedRows[0][lblNumTotal];
                            curAnsCount = getAnswerCounts(selectedRows[0]);

                            resultsTable.Rows.Remove(selectedRows[0]);
                        }
                        else
                        {
                            curNumCorrect = 0;
                            curNumTotal = 0;
                            curAnsCount.Reset();
                        }

                        // add the row to the results table
                        DataRow thisrow = resultsTable.NewRow();
                        //thisrow[lblCampus] = dvStudentData.Table.Rows[j]["SCHOOL_ABBR"].ToString();
                        thisrow[lblCampus] = dvStudentData.Table.Rows[jj]["SCHOOL2"].ToString();
                        thisrow[lblTestId] = curTest;
                        thisrow[lblTeacher] = dvStudentData.Table.Rows[jj][birIF.teacherNameFieldName].ToString();
                        thisrow[lblPeriod] = dvStudentData.Table.Rows[jj]["PERIOD"].ToString();
                        thisrow[lblItemNum] = gradedTable.Rows[k]["ITEM_NUM"];
                        if ((bool)gradedTable.Rows[k]["CORRECT"])
                            curNumCorrect++;
                        curAnsCount.Increment(gradedTable.Rows[k]["STUDENT_ANS"].ToString());                        
                        curNumTotal++;
                        thisrow[lblNumCorrect] = curNumCorrect;
                        thisrow[lblNumTotal] = curNumTotal;
                        thisrow[lblPctCorrect] = (decimal)curNumCorrect / (decimal)curNumTotal;
                        putAnswerCounts(curAnsCount, thisrow);
                        thisrow[lblAnswer] = gradedTable.Rows[k]["CORRECT_ANS"].ToString();
                        thisrow[lblObjective] = gradedTable.Rows[k]["OBJECTIVE"];
                        thisrow[lblTEKS] = gradedTable.Rows[k]["TEKS"];

                        resultsTable.Rows.Add(thisrow);
                    }

                }
                else
                {
                    numNull++;
                }
            }

            return resultsTable;
        }


        //**********************************************************************//
        //** write the table of current results to the temporary results
        //** data table so that the report can use it
        //**
        public static int writeBenchmarkStatsResultsToDb(DataTable resultsTable)
        {
            int result = new int();
            DataSet ds = new DataSet();
            string qs = "SELECT * FROM " + benchmarkStatsResultsDatatableName;
            string findcriteria1 = "";

            // get connection to the database
            string connectionString = ConfigurationManager.ConnectionStrings[dbIFOracle.databaseName].ConnectionString;
            using (OracleConnection connection =
                new OracleConnection(connectionString))
            {
                OracleDataAdapter adapter = new OracleDataAdapter();
                adapter.SelectCommand = new OracleCommand(qs, connection);
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                OracleCommandBuilder cmdb = new OracleCommandBuilder(adapter);
                adapter.Fill(ds, benchmarkStatsResultsDatatableName);

                // write the data to the DataSet
                foreach (DataRow row in resultsTable.Rows)
                {
                    // check and see if current record already exists and is the latest one
                    findcriteria1 = "CAMPUS = \'" + row["CAMPUS"].ToString() + "\' " +
                                    "AND TEST_ID = \'" + row["TEST_ID"].ToString() + "\' " +
                                    "AND TEACHER = \'" + row["TEACHER"].ToString().Replace("'", "''") + "\' " +
                                    "AND PERIOD = \'" + row["PERIOD"].ToString() + "\' " +
                                    "AND ITEM_NUM = " + row["ITEM_NUM"].ToString();


                    int l = ds.Tables[0].Select(findcriteria1).Length;
                    if (l == 0)                 // row not there, add it to the dataset
                    {
                        DataRow row2 = ds.Tables[0].NewRow();
                        row2.ItemArray = row.ItemArray;
                        ds.Tables[0].Rows.Add(row2);
                    }
                    else                        // row is there, overwrite it if it's different data 
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if ((ds.Tables[0].Rows[i]["CAMPUS"].ToString() == row["CAMPUS"].ToString())
                                && (ds.Tables[0].Rows[i]["TEST_ID"].ToString() == row["TEST_ID"].ToString())
                                && (ds.Tables[0].Rows[i]["TEACHER"].ToString() == row["TEACHER"].ToString())
                                && (ds.Tables[0].Rows[i]["PERIOD"].ToString() == row["PERIOD"].ToString())
                                && (ds.Tables[0].Rows[i]["ITEM_NUM"].ToString() == row["ITEM_NUM"].ToString()))
                            {
                                if (ds.Tables[0].Rows[i].ItemArray != row.ItemArray)
                                {
                                    // overwrite the existing row with this one
                                    ds.Tables[0].Rows[i].ItemArray = row.ItemArray;
                                }
                            }
                            // otherwise just keep the existing one and move on
                        }
                    }
                }
                adapter.Update(ds, benchmarkStatsResultsDatatableName);
            }

            result = 1;

            return result;
        }


        //**********************************************************************//
        //** create columns for the results data table
        //**
        private static void setupResultsDataTable(DataTable rtable)
        {
            
            
            // create columns for the resulting table
            rtable.Columns.Add(new DataColumn(lblCampus, System.Type.GetType("System.String")));
            rtable.Columns.Add(new DataColumn(lblTestId, System.Type.GetType("System.String")));
            rtable.Columns.Add(new DataColumn(lblTeacher, System.Type.GetType("System.String")));
            rtable.Columns.Add(new DataColumn(lblPeriod, System.Type.GetType("System.String")));
            rtable.Columns.Add(new DataColumn(lblItemNum, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblPctCorrect, System.Type.GetType("System.Decimal")));
            rtable.Columns.Add(new DataColumn(lblNumCorrect, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumTotal, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumA, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumB, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumC, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumD, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumE, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumF, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumG, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumH, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumJ, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumK, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblAnswer, System.Type.GetType("System.String")));
            rtable.Columns.Add(new DataColumn(lblObjective, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblTEKS, System.Type.GetType("System.String")));

        }


        //**********************************************************************//
        //** get the current values of answers from the results data table,
        //** return them in an answerCounter class structure
        //**
        private static answerCounter getAnswerCounts(DataRow row)
        {
            answerCounter c = new answerCounter();

            c.a = (int)row[lblNumA];
            c.b = (int)row[lblNumB];
            c.c = (int)row[lblNumC];
            c.d = (int)row[lblNumD];
            c.e = (int)row[lblNumE];
            c.f = (int)row[lblNumF];
            c.g = (int)row[lblNumG];
            c.h = (int)row[lblNumH];
            c.j = (int)row[lblNumJ];
            c.k = (int)row[lblNumK];

            return c;
        }


        //**********************************************************************//
        //** put the current values of answers from the answerCounter class
        //** instance into a data row
        //**
        private static void putAnswerCounts(answerCounter ac, DataRow row)
        {
            row[lblNumA] = ac.a;
            row[lblNumB] = ac.b;
            row[lblNumC] = ac.c;
            row[lblNumD] = ac.d;
            row[lblNumE] = ac.e;
            row[lblNumF] = ac.f;
            row[lblNumG] = ac.g;
            row[lblNumH] = ac.h;
            row[lblNumJ] = ac.j;
            row[lblNumK] = ac.k;

            return;
        }


    }
}