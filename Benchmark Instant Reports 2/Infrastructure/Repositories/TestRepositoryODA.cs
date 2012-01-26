using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.References;
using System.Data;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    public class TestRepositoryODA : ITestRepository
    {
        public Test FindByTestID(string testid)
        {
            string qs = Queries.GetTestInfoForTest.Replace("@testId", testid);
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToTest(ds.Tables[0].Rows[0]);
        }
        

        public IQueryable<Test> FindAll()
        {
            string qs = Queries.GetTestInfoForAll;
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToTests(ds.Tables[0]);
        }


        public IQueryable<Test> FindWhere(Func<Test, bool> predicate)
        {
            return FindAll().Where(predicate).AsQueryable();
        }



        public void Add(Test newentity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Test entity)
        {
            throw new NotImplementedException();
        }




        private static Test ConvertRowToTest(DataRow row)
        {
            Test retTest = new Test();
            retTest.TestID = row["TEST_ID"].ToString();
            retTest.TestYear = (row["TEST_YEAR"].ToString() != "") ? Convert.ToInt32(row["TEST_YEAR"].ToString()) : 0;
            retTest.TestMonth = (row["TEST_MONTH"].ToString() != "") ? Convert.ToInt32(row["TEST_MONTH"].ToString()) : 0;
            retTest.StartDate = (row["START_DATE"].ToString() != "") 
                ? DateTime.Parse(row["START_DATE"].ToString()) 
                : DateTime.Parse("1/1/1990"); 
            retTest.EndDate = (row["END_DATE"].ToString() != "")
                ? DateTime.Parse(row["END_DATE"].ToString()) 
                : DateTime.Parse("1/1/1990"); 
            retTest.Language = row["LANGUAGE_VERSION"].ToString();
            retTest.Subject = row["TEST_SUBJECT"].ToString();
            retTest.Title = row["TEST_TITLE"].ToString();
            retTest.Grade = (row["TEST_GRADE"].ToString() != "") ? Convert.ToInt32(row["TEST_GRADE"].ToString()) : 0;
            retTest.NumItems = (row["NUM_ITEMS"].ToString() != "") ? Convert.ToInt32(row["NUM_ITEMS"].ToString()) : 0;
            retTest.NumPoints = (row["NUM_POINTS"].ToString() != "") ? Convert.ToInt32(row["NUM_POINTS"].ToString()) : 0;
            retTest.PassNum = (row["PASS_NUM"].ToString() != "") ? Convert.ToInt32(row["PASS_NUM"].ToString()) : 0;
            retTest.CommendedNum = (row["COMMENDED_NUM"].ToString() != "") ? Convert.ToInt32(row["COMMENDED_NUM"].ToString()) : 0;
            retTest.CompScore = (row["COMP_SCORE"].ToString() != "") ? Convert.ToInt32(row["COMP_SCORE"].ToString()) : 0;
            retTest.Projection = (row["PROJECTION"].ToString() != "") ? Convert.ToInt32(row["PROJECTION"].ToString()) : 0;
            retTest.TestTemplate = row["TEST_TEMPLATE"].ToString();
            retTest.CustomQueryOrig = row["CUSTOM_QUERY_ORIG"].ToString();
            retTest.TestIDNum = (row["TEST_ID_NBR"].ToString() != "") ? Convert.ToInt32(row["TEST_ID_NBR"].ToString()) : 0;
            retTest.ActualStartDate = (row["ACTUAL_START_DATE"].ToString() != "")
                ? DateTime.Parse(row["ACTUAL_START_DATE"].ToString())
                : DateTime.Parse("1/1/1990");
            retTest.ActualEndDate = (row["ACTUAL_END_DATE"].ToString() != "")
                ? DateTime.Parse(row["ACTUAL_END_DATE"].ToString())
                : DateTime.Parse("1/1/1990"); 
            retTest.SchoolType = row["SCHOOL_TYPE"].ToString();
            retTest.CustomQuery = row["CUSTOM_QUERY"].ToString();
            retTest.SecSchoolType = row["SEC_SCHOOL_TYPE"].ToString();

            return retTest;
        }


        private static IQueryable<Test> ConvertRowToTests(DataTable table)
        {
            HashSet<Test> finaldata = new HashSet<Test>();
            foreach (DataRow row in table.Rows)
            {
                Test newTest = ConvertRowToTest(row);
                finaldata.Add(newTest);
            }

            return finaldata.AsQueryable();
        }

    }
}