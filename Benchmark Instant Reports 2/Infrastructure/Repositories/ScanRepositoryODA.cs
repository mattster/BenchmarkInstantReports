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
            retScan.DateScanned = DateTime.Parse(row["DATE_SCANNED"].ToString());
            retScan.ScanSequence = (row["SCANNED_SEQUENCE"].ToString() != "") 
                ? Convert.ToInt32(row["SCANNED_SEQUENCE"].ToString()) 
                : 0;
            retScan.Imagepath = row["IMAGEPATH"].ToString();
            retScan.StudentName = row["NAME"].ToString();
            retScan.StudentID = (row["STUDENT_ID"].ToString() != "") ? Convert.ToInt32(row["STUDENT_ID"].ToString()) : 0;
            retScan.TestID = row["TEST_ID"].ToString();
            retScan.Language = row["LANGUAGE_VERSION"].ToString();
            retScan.Exempt = row["EXEMPT"].ToString();
            retScan.Preslugged = row["PRESLUGGED"].ToString();
            retScan.AnswerString = row["ANSWERS"].ToString();

            return retScan;
        }


        private static IQueryable<Scan> ConvertRowToScans(DataTable table)
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