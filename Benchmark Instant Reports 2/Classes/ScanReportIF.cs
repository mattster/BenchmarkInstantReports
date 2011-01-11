using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;

//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
// an interface class for the Scan Report report
//
// matt hollingsworth
// oct 2010
//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *


namespace Benchmark_Instant_Reports_2
{
    public class ScanReportIF
    {
        public static string scanReportResultsDatatableName = "ACI.TEMP_RESULTS_SCANREPORT";
        private static string lblCampus = "CAMPUS";
        private static string lblTestId = "TEST_ID";
        private static string lblTeacher = "TEACHER";
        private static string lblPeriod = "PERIOD";
        private static string lblNumScanned = "NUM_SCANNED";
        private static string lblNumQueried = "NUM_QUERIED";
        private static string lblSchoolAbbr = "SCHOOL_ABBR";
        private static string lblStudentId = "STUDENT_ID";
        private static string lblLocalStudentId = "LOCAL_STUDENT_ID";


        public class TestQueryContainer
        {
            public string testid;
            public DataSet querydataset;

            //default constructor
            public TestQueryContainer()
            {
                testid = "";
                querydataset = new DataSet();
            }

            public TestQueryContainer(string id, DataSet ds)
            {
                testid = id;
                querydataset = ds;
            }
        }




        //**********************************************************************//
        //** generate a DataTable to be used for the Scan report
        //** for the specified Campus and Benchmarks
        //**
        //** DataTable.Columns :
        //** (campus, test_id, teacher, period, num_scanned, num_queried)
        //**
        public static DataTable generateScanRepTable(string campus, string[] benchmarks)
        {
            DataTable table = new DataTable();

            // create columns for the results table
            table.Columns.Add(new DataColumn(lblCampus, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblTestId, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblTeacher, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblPeriod, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblNumScanned, System.Type.GetType("System.Int32")));
            table.Columns.Add(new DataColumn(lblNumQueried, System.Type.GetType("System.Int32")));

            //// get datasets with the custom queries for each test selected
            //TestQueryContainer[] testQueries = new TestQueryContainer[benchmarks.Length];
            //for (int i = 0; i < benchmarks.Length; i++)
            //{
            //    testQueries[i] = new TestQueryContainer(benchmarks[i], birIF.executeStudentListQuery(benchmarks[i], campus));
            //}

            // get student scanned data for the specified tests
            DataSet dsStudentScans = birIF.getStudentScanListData(benchmarks[0], campus);

            // if there are more than 1 benchmark, get the rest of the student scan data
            if (benchmarks.Length > 1)
            {
                for (int ii = 1; ii < benchmarks.Length; ii++)              // we already did benchmarks[0]
                {
                    DataSet tempds = birIF.getStudentScanListData(benchmarks[ii], campus);
                    dsStudentScans.Merge(tempds, true, MissingSchemaAction.Add);
                }
            }

            // go through the list of scanned records
            for (int j = 0; j < dsStudentScans.Tables[0].Rows.Count; j++)
            {
                DataRow newrow = table.NewRow();
                newrow[lblCampus] = campus;
                newrow[lblTestId] = dsStudentScans.Tables[0].Rows[j][lblTestId].ToString();
                newrow[lblTeacher] = dsStudentScans.Tables[0].Rows[j][birIF.teacherNameFieldName].ToString();
                newrow[lblPeriod] = dsStudentScans.Tables[0].Rows[j][lblPeriod].ToString();
                newrow[lblNumScanned] = 1;
                newrow[lblNumQueried] = 0;                              // we'll count this later
                addRowToScanResultsTable(newrow, table);
            }

            
            #region countqueries
            // go through list of queried records to get the queried counts
            //for (int k1 = 0; k1 < testQueries.Length; k1++)
            //{
            //    curTestId = testQueries[k1].testid;
            //    string[] listOfTeachers = birUtilities.getUniqueTableColumnStringValues(testQueries[k1].querydataset.Tables[0],
            //                        birIF.teacherNameFieldName);
                
            //    // go through each teacher and count numbers for each period
            //    for (int k2 = 0; k2 < listOfTeachers.Length; k2++)
            //    {
            //        string teacherFilter = birIF.teacherNameFieldName + " = \'" + listOfTeachers[k2] + "\'";
            //        DataView queryDataForTeacher = new DataView(testQueries[k1].querydataset.Tables[0], teacherFilter,
            //            lblPeriod + " ASC", DataViewRowState.CurrentRows);
            //        string[] listOfPeriods = birUtilities.getUniqueTableColumnStringValues(queryDataForTeacher.ToTable(), lblPeriod);

            //        // go through each period and count the number of records
            //        for (int k3 = 0; k3 < listOfPeriods.Length; k3++)
            //        {
            //            string periodFilter = lblPeriod + " = \'" + listOfPeriods[k3] + "\'";
            //            //DataView queryDataForPeriod = new DataView(queryDataForTeacher.ToTable(), periodFilter,
            //            //    "STUDENT_ID ASC", DataViewRowState.CurrentRows);
            //            curNumQueried = queryDataForTeacher.ToTable().Select(periodFilter).Length;

            //            // write the data to the results table
            //            DataRow newrow2 = table.NewRow();
            //            newrow2[lblCampus] = queryDataForTeacher.Table.Rows[0][lblSchoolAbbr].ToString();
            //            newrow2[lblTestId] = curTestId;
            //            newrow2[lblTeacher] = listOfTeachers[k2];
            //            newrow2[lblPeriod] = listOfPeriods[k3];
            //            newrow2[lblNumScanned] = 0;                     // already counted above
            //            newrow2[lblNumQueried] = 1;
            //            addRowToScanResultsTable(newrow2, table);
            //        }
            //    }
            //}
            #endregion


            return table;
        }

        
        //**********************************************************************//
        //** write data table of Scan Report results to the temp results
        //** database table so that the report can use it
        //**
        public static int writeScanReportResultsToDb(DataTable resultsTable)
        {
            int result = new int();
            DataSet ds = new DataSet();
            string qs = "SELECT * FROM " + scanReportResultsDatatableName;
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
                adapter.Fill(ds, scanReportResultsDatatableName);

                // write the data to the DataSet
                foreach (DataRow row in resultsTable.Rows)
                {
                    // check and see if current record already exists and is the latest one
                    findcriteria1 = lblCampus + " = \'" + row[lblCampus].ToString() + "\' " +
                                    "AND " + lblTestId + " = \'" + row[lblTestId].ToString() + "\' " +
                                    "AND " + lblTeacher + " = \'" + row[lblTeacher].ToString() + "\' " +
                                    "AND " + lblPeriod + " = \'" + row[lblPeriod].ToString() + "\'";
                    
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
                            if ((ds.Tables[0].Rows[i][lblCampus].ToString() == row[lblCampus].ToString())
                                && (ds.Tables[0].Rows[i][lblTestId].ToString() == row[lblTestId].ToString())
                                && (ds.Tables[0].Rows[i][lblTeacher].ToString() == row[lblTeacher].ToString())
                                && (ds.Tables[0].Rows[i][lblPeriod].ToString() == row[lblPeriod].ToString()))
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
                adapter.Update(ds, scanReportResultsDatatableName);
            }

            result = 1;

            return result;
        }
        

        //**********************************************************************//
        //** returns the index number of the array of TestQeryContainers 
        //** where the testid is that of the specified testid
        //** 
        //**
        private static int getDsIndexOfTest(TestQueryContainer[] dsAry, string id)
        {
            for (int i = 0; i < dsAry.Length; i++)
            {
                if (dsAry[i].testid == id)
                    return i;
            }

            return -1;
        }


        //**********************************************************************//
        //** write the specified datarow to the specified results table;
        //** check and see if the campus-test_id-teacher-period combination
        //** is already there, and if so, get the value of NUM_SCANNED and add
        //** it to the value of that field in the specified row
        //**
        private static void addRowToScanResultsTable(DataRow newRow, DataTable resultsTable)
        {
            string findCriteria = "";
            int curNumScanned = new int();
            int curNumQueried = new int();

            string curCampus = newRow[lblCampus].ToString();
            string curTestId = newRow[lblTestId].ToString();
            string curTeacher = newRow[lblTeacher].ToString();
            string curPeriod = newRow[lblPeriod].ToString();
            curNumScanned = (int)newRow[lblNumScanned];
            curNumQueried = (int)newRow[lblNumQueried];

            // do we already have this row in the results table?
            findCriteria =
                lblCampus + " = \'" + curCampus + "\' " +
                "AND " + lblTestId + " = \'" + curTestId + "\' " +
                "AND " + lblTeacher + " = \'" + curTeacher + "\' " +
                "AND " + lblPeriod + " = \'" + curPeriod + "\'";
            DataRow[] foundRows = resultsTable.Select(findCriteria);
            if (foundRows.Length > 0)
            {
                // this row is here - get the values then delete it
                curNumScanned += (int)foundRows[0][lblNumScanned];
                curNumQueried += (int)foundRows[0][lblNumQueried];
                resultsTable.Rows.Remove(foundRows[0]);
            }
            else
            {
                // this row is not yet here - we don't need to do anything
                
            }

            // add the row to the results table
            DataRow tempRow = resultsTable.NewRow();
            tempRow[lblCampus] = curCampus;
            tempRow[lblTestId] = curTestId;
            tempRow[lblTeacher] = curTeacher;
            tempRow[lblPeriod] = curPeriod;
            tempRow[lblNumScanned] = curNumScanned;
            tempRow[lblNumQueried] = curNumQueried;
            resultsTable.Rows.Add(tempRow);

            return;
        }
    
    
    
    }
}