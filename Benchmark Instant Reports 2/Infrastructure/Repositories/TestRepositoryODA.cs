using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.References;

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


        public IQueryable<Test> FindActiveTests()
        {
            //return FindWhere(t => t.StartDate >= Constants.FirstDaySchoolYear &&
            //                      t.StartDate <= DateTime.Today &&
            //                      t.Subject != Constants.TestSubjectBallot &&
            //                      t.Subject != Constants.TestSubjectSample);
            string qs = Queries.GetTestListAllTests;
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToTests(ds.Tables[0]);
        }


        public IQueryable<Test> FindAll()
        {
            string qs = Queries.GetTestInfoForAll;
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToTests(ds.Tables[0]);
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
            retTest.TestID = ODAHelper.GetTableValueSafely(row, "TEST_ID").ToString();
            retTest.TestYear = (ODAHelper.GetTableValueSafely(row, "TEST_YEAR").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "TEST_YEAR").ToString()) 
                : 0;
            retTest.TestMonth = (ODAHelper.GetTableValueSafely(row, "TEST_MONTH").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "TEST_MONTH").ToString()) 
                : 0;
            retTest.StartDate = (ODAHelper.GetTableValueSafely(row, "START_DATETIME").ToString() != "") 
                ? DateTime.Parse(ODAHelper.GetTableValueSafely(row, "START_DATETIME").ToString()) 
                : DateTime.Parse("1/1/1990"); 
            retTest.EndDate = (ODAHelper.GetTableValueSafely(row, "END_DATETIME").ToString() != "")
                ? DateTime.Parse(ODAHelper.GetTableValueSafely(row, "END_DATETIME").ToString()) 
                : DateTime.Parse("1/1/1990"); 
            retTest.Language = ODAHelper.GetTableValueSafely(row, "LANGUAGE_VERSION").ToString();
            retTest.Subject = ODAHelper.GetTableValueSafely(row, "TEST_SUBJECT").ToString();
            retTest.Title = ODAHelper.GetTableValueSafely(row, "TEST_TITLE").ToString();
            retTest.Grade = (ODAHelper.GetTableValueSafely(row, "TEST_GRADE").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "TEST_GRADE").ToString()) 
                : 0;
            retTest.NumItems = (ODAHelper.GetTableValueSafely(row, "NUM_ITEMS").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "NUM_ITEMS").ToString()) 
                : 0;
            retTest.NumPoints = (ODAHelper.GetTableValueSafely(row, "NUM_POINTS").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "NUM_POINTS").ToString()) 
                : 0;
            retTest.PassNum = (ODAHelper.GetTableValueSafely(row, "PASS_NUM").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "PASS_NUM").ToString()) 
                : 0;
            retTest.CommendedNum = (ODAHelper.GetTableValueSafely(row, "COMMENDED_NUM").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "COMMENDED_NUM").ToString()) 
                : 0;
            retTest.CompScore = (ODAHelper.GetTableValueSafely(row, "COMP_SCORE").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "COMP_SCORE").ToString()) 
                : 0;
            retTest.Projection = (ODAHelper.GetTableValueSafely(row, "PROJECTION").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "PROJECTION").ToString()) 
                : 0;
            retTest.TestTemplate = ODAHelper.GetTableValueSafely(row, "TEST_TEMPLATE").ToString();
            retTest.CustomQueryOrig = ODAHelper.GetTableValueSafely(row, "CUSTOM_QUERY_ORIG").ToString();
            retTest.TestIDNum = (ODAHelper.GetTableValueSafely(row, "TEST_ID_NBR").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "TEST_ID_NBR").ToString()) 
                : 0;
            retTest.ActualStartDate = (ODAHelper.GetTableValueSafely(row, "ACTUAL_START_DATE").ToString() != "")
                ? DateTime.Parse(ODAHelper.GetTableValueSafely(row, "ACTUAL_START_DATE").ToString())
                : DateTime.Parse("1/1/1990");
            retTest.ActualEndDate = (ODAHelper.GetTableValueSafely(row, "ACTUAL_END_DATE").ToString() != "")
                ? DateTime.Parse(ODAHelper.GetTableValueSafely(row, "ACTUAL_END_DATE").ToString())
                : DateTime.Parse("1/1/1990");

            string schType = ODAHelper.GetTableValueSafely(row, "SCHOOL_TYPE").ToString();
            if (schType == "E")
                retTest.SchoolType = Constants.SchoolType.Elementary;
            else if (schType == "S")
                retTest.SchoolType = Constants.SchoolType.AllSecondary;
            else retTest.SchoolType = Constants.SchoolType.All;
            
            retTest.CustomQuery = ODAHelper.GetTableValueSafely(row, "CUSTOM_QUERY").ToString();

            string secSchType = ODAHelper.GetTableValueSafely(row, "SEC_SCHOOL_TYPE").ToString();
            if (secSchType == "J")
                retTest.SecSchoolType = Constants.SchoolType.JuniorHigh;
            else if (secSchType == "H")
                retTest.SecSchoolType = Constants.SchoolType.HighSchool;
            else retTest.SecSchoolType = Constants.SchoolType.Unknown;

            return retTest;
        }


        private static IQueryable<Test> ConvertTableToTests(DataTable table)
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