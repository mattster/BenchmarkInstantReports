using System;
using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    public class ScanRepositoryDapper : IScanRepository
    {
        public IQueryable<Scan> FindScansForTest(string testid)
        {
            string qs = Queries.GetScansForTest;
            qs = qs.Replace("@testId", testid);
            var results = DapperHelper.DQuery(qs);
            return ConvertToScanQ(results);
        }


        public IQueryable<Scan> FindLatestScansForTest(string testid)
        {
            string qs = Queries.GetLatestScansForTest;
            qs = qs.Replace("@testId", testid);
            var results = DapperHelper.DQuery(qs);
            return ConvertToScanQ(results);
        }


        public IQueryable<Scan> FindScansForTestCampus(string testid, string abbr)
        {
            string qs = Queries.GetScansForTestCampus;
            qs = qs.Replace("@testId", testid);
            qs = qs.Replace("@campus", abbr);
            var results = DapperHelper.DQuery(qs);
            return ConvertToScanQ(results);
        }



        public IQueryable<Scan> FindAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Scan> FindWhere(Func<Entities.Scan, bool> predicate)
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




        private static IQueryable<Scan> ConvertToScanQ(IEnumerable<dynamic> rawdata)
        {
            HashSet<Scan> finalData = new HashSet<Scan>();

            foreach (var item in rawdata)
            {
                Scan newScan = new Scan();
                newScan.DateScanned = DateTime.Parse(item.DATE_SCANNED as string);
                newScan.ScanSequence = Convert.ToInt32((item.SCANNED_SEQUENCE).ToString());
                newScan.Imagepath = item.IMAGEPATH;
                newScan.StudentName = item.NAME;
                newScan.StudentID = Convert.ToInt32((item.STUDENT_ID).ToString());
                newScan.TestID = item.TEST_ID;
                newScan.Language = (item.LANGUAGE_VERSION).ToString();
                newScan.Exempt = (item.EXEMPT).ToString();
                newScan.Preslugged = (item.PRESLUGGED).ToString();
                newScan.AnswerString = item.ANSWERS;
                finalData.Add(newScan);
            }

            return finalData.AsQueryable();
        }

    
    
    }
}