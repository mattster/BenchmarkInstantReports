using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Benchmark_Instant_Reports_2.Exceptions;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;



namespace Benchmark_Instant_Reports_2.Interfaces
{
    public class birIF
    {
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

            return query;
        }


        public static ScanItem getLatestScanDataRowq(string studentId, string testId)
        {
            string q = Queries.GetLatestScanForStudent;

            q = q.Replace("@studentId", studentId);
            q = q.Replace("@testId", testId);

            return DBIOWorkaround.ReturnScanItemFromQ(q);

        }


        public static int getTestPassingNum(string theTest)
        {
            string qs = Queries.GetPassNum.Replace("@testId", theTest);
            DataSet ds = dbIFOracle.getDataRows(qs);

            decimal nd = (decimal)ds.Tables[0].Rows[0][0];
            return (int)nd;
        }


        public static int getTestCommendedNum(string theTest)
        {
            string qs = Queries.GetCommendedNum.Replace("@testId", theTest);
            DataSet ds = dbIFOracle.getDataRows(qs);

            decimal nd = (decimal)ds.Tables[0].Rows[0][0];
            return (int)nd;
        }


        internal static bool isStudentElemNoAttCourse(string studentId)
        {
            string qs = Queries.GetElemStudentsNotInAttCourse.Replace("@studentId", studentId);
            DataSet ds = dbIFOracle.getDataRows(qs);

            if (ds.Tables.Count > 0)
                return true;

            return false;
        }
    }
}