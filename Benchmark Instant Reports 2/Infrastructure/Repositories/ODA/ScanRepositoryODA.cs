using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    /// <summary>
    /// implementation of the Scan repository using Oracle Data Access
    /// </summary>
    public class ScanRepositoryODA : IScanRepository
    {
        /// <summary>
        /// finds all the Scans for a specific test
        /// </summary>
        /// <param name="testid">TestID of the test to use</param>
        /// <returns>IQueryable-Scan- list of data objects</returns>
        public IQueryable<Scan> FindScansForTest(string testid)
        {
            string qs = Queries.GetScansForTest;
            qs = qs.Replace("@testId", testid);
            DataSet ds = ODAHelper.getDataRows(qs);

            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToScans(ds.Tables[0]);
        }



        public IQueryable<Scan> FindLatestScansForTest(string testid)
        {
            string qs = Queries.GetLatestScansForTest;
            qs = qs.Replace("@testId", testid);
            DataSet ds = ODAHelper.getDataRows(qs);

            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToScans(ds.Tables[0]);
        }


        /// <summary>
        /// finds all the Scans for a specific test for students at
        /// a specified school
        /// </summary>
        /// <param name="testid">TestID of the test to use</param>
        /// <param name="schoolAbbr">School abbreviation of the school to use</param>
        /// <returns>IQueryable-Scan- list of data objects</returns>
        public IQueryable<Scan> FindScansForTestCampus(string testid, string schoolAbbr)
        {
            string qs = Queries.GetScansForTestCampus;
            qs = qs.Replace("@testId", testid);
            qs = qs.Replace("@campus", schoolAbbr);
            DataSet ds = ODAHelper.getDataRows(qs);

            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToScans(ds.Tables[0]);
        }
        
        


        public IQueryable<Scan> FindAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Scan> FindWhere(Func<Scan, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(Scan newentity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Scan entity)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// converts a row read in from the database to a Scan object
        /// </summary>
        /// <param name="row">DataRow read in from the database</param>
        /// <returns>a Scan object with the data</returns>
        private static Scan ConvertRowToScan(DataRow row)
        {
            Scan retScan = new Scan();
            retScan.DateScanned = DateTime.Parse(ODAHelper.GetTableValueSafely(row, "DATE_SCANNED").ToString());
            retScan.ScanSequence = (ODAHelper.GetTableValueSafely(row, "SCANNED_SEQUENCE").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "SCANNED_SEQUENCE").ToString()) 
                : 0;
            retScan.Imagepath = ODAHelper.GetTableValueSafely(row, "IMAGEPATH").ToString();
            retScan.StudentName = ODAHelper.GetTableValueSafely(row, "NAME").ToString();
            retScan.StudentID = (ODAHelper.GetTableValueSafely(row, "STUDENT_ID").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "STUDENT_ID").ToString()) 
                : 0;
            retScan.TestID = ODAHelper.GetTableValueSafely(row, "TEST_ID").ToString();
            retScan.Language = ODAHelper.GetTableValueSafely(row, "LANGUAGE_VERSION").ToString();
            retScan.Exempt = ODAHelper.GetTableValueSafely(row, "EXEMPT").ToString();
            retScan.Preslugged = ODAHelper.GetTableValueSafely(row, "PRESLUGGED").ToString();
            retScan.AnswerString = ODAHelper.GetTableValueSafely(row, "ANSWERS").ToString();

            return retScan;
        }


        /// <summary>
        /// converts a table of data read in from the database to an 
        /// IQueryable-Scan- list of objects
        /// </summary>
        /// <param name="table">DataTable read in from the database</param>
        /// <returns>IQueryable-Scan- list of data objects</returns>
        private static IQueryable<Scan> ConvertTableToScans(DataTable table)
        {
            HashSet<Scan> finaldata = new HashSet<Scan>();
            foreach (DataRow row in table.Rows)
            {
                Scan newScan = ConvertRowToScan(row);
                finaldata.Add(newScan);
            }

            return finaldata.AsQueryable();
        }

    }
}