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
    public class ScanRepositoryODA : IScanRepository
    {
        public IQueryable<Scan> FindScansForTestCampus(string testid, string abbr)
        {
            string qs = Queries.GetScansForTestCampus;
            qs = qs.Replace("@testId", testid);
            qs = qs.Replace("@campus", abbr);
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