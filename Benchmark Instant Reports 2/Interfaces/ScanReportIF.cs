using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using Benchmark_Instant_Reports_2.References;


namespace Benchmark_Instant_Reports_2.Interfaces
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


        public static DataTable generateQueryRepTable(string campus, string[] benchmarks)
        {
            DataTable table = new DataTable();

            // create columns for the results table
            table.Columns.Add(new DataColumn(lblCampus, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblTestId, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblTeacher, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblPeriod, System.Type.GetType("System.String")));
            table.Columns.Add(new DataColumn(lblNumScanned, System.Type.GetType("System.Int32")));
            table.Columns.Add(new DataColumn(lblNumQueried, System.Type.GetType("System.Int32")));


            // get data for number queried for the specified tests
            DataSet dsQueried = birIF.executeStudentListQuery(benchmarks[0], campus);
            // go through the list of queried records
            for (int k = 0; k < dsQueried.Tables[0].Rows.Count; k++)
            {
                DataRow newrow = table.NewRow();
                newrow[lblCampus] = dsQueried.Tables[0].Rows[k]["SCHOOL2"].ToString();
                newrow[lblTestId] = benchmarks[0];
                newrow[lblTeacher] = dsQueried.Tables[0].Rows[k][Constants.TeacherNameFieldName].ToString();
                newrow[lblPeriod] = dsQueried.Tables[0].Rows[k][lblPeriod].ToString();
                newrow[lblNumScanned] = 0;                                  // we'll count this later
                newrow[lblNumQueried] = 1;
                addRowToScanResultsTable(newrow, table);
            }


            // if there is more than 1 benchmark, get the rest of the queried data
            if (benchmarks.Length > 1)
            {
                for (int i = 1; i < benchmarks.Length; i++)                 // we already did benchmarks[0]
                {
                    DataSet tempds = birIF.executeStudentListQuery(benchmarks[i], campus);
                    //dsQueried.Merge(tempds, true, MissingSchemaAction.Add);
                    //joinDS(dsQueried, tempds);

                    // go through the list of queried records
                    for (int k = 0; k < tempds.Tables[0].Rows.Count; k++)
                    {
                        DataRow newrow = table.NewRow();
                        newrow[lblCampus] = tempds.Tables[0].Rows[k]["SCHOOL2"].ToString();
                        newrow[lblTestId] = benchmarks[i];
                        newrow[lblTeacher] = tempds.Tables[0].Rows[k][Constants.TeacherNameFieldName].ToString();
                        newrow[lblPeriod] = tempds.Tables[0].Rows[k][lblPeriod].ToString();
                        newrow[lblNumScanned] = 0;                                  // we'll count this later
                        newrow[lblNumQueried] = 1;
                        addRowToScanResultsTable(newrow, table);
                    }

                }
            }

            return table;
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
            string qcurTeacher = curTeacher.Replace("'", "''");
            findCriteria =
                lblCampus + " = \'" + curCampus + "\' " +
                "AND " + lblTestId + " = \'" + curTestId + "\' " +
                "AND " + lblTeacher + " = \'" + qcurTeacher + "\' " +
                "AND " + lblPeriod + " = \'" + curPeriod + "\'";
            DataRow[] foundRows = resultsTable.Select(findCriteria);
            if (foundRows.Length > 0)
            {
                for (int i = 0; i < foundRows.Length; i++)
                {
                    // there are rows here - get the values then delete it
                    curNumScanned += (int)foundRows[i][lblNumScanned];
                    curNumQueried += (int)foundRows[i][lblNumQueried];
                    resultsTable.Rows.Remove(foundRows[i]);
                }
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