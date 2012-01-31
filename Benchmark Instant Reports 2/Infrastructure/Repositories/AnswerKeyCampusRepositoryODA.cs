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
    public class AnswerKeyCampusRepositoryODA : IAnswerKeyCampusRepository
    {

        public IQueryable<AnswerKeyCampus> FindKeyForTest(string testid, string campusabbr)
        {
            string qs = Queries.GetCampusTestAnswerKey.Replace("@testId", testid);
            qs = qs.Replace("@schoolAbbr", campusabbr);
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToAKCs(ds.Tables[0]);
        }

        public System.Linq.IQueryable<AnswerKeyCampus> FindAll()
        {
            string qs = Queries.GetAllCampusAnswerKeys;
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToAKCs(ds.Tables[0]);
        }




        public System.Linq.IQueryable<AnswerKeyCampus> FindWhere(Func<AnswerKeyCampus, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(AnswerKeyCampus newentity)
        {
            throw new NotImplementedException();
        }

        public void Remove(AnswerKeyCampus entity)
        {
            throw new NotImplementedException();
        }





        private static AnswerKeyCampus ConvertRowToAKC(DataRow row)
        {
            AnswerKeyCampus retAKC = new AnswerKeyCampus();
            retAKC.TestID = ODAHelper.GetTableValueSafely(row, "TEST_ID").ToString();
            retAKC.SchoolAbbr = ODAHelper.GetTableValueSafely(row, "SCHOOL_ABBR").ToString();
            retAKC.ItemNum = (ODAHelper.GetTableValueSafely(row, "ITEM_NUM").ToString() != "")
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "ITEM_NUM").ToString()) 
                : 0;
            retAKC.Answer = ODAHelper.GetTableValueSafely(row, "ANSWER").ToString();
            retAKC.Objective = (ODAHelper.GetTableValueSafely(row, "OBJECTIVE").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "OBJECTIVE").ToString()) 
                : 0;
            retAKC.TEKS = ODAHelper.GetTableValueSafely(row, "TEKS").ToString();
            retAKC.Weight = (ODAHelper.GetTableValueSafely(row, "WEIGHT").ToString() != "") 
                ? Convert.ToDouble(ODAHelper.GetTableValueSafely(row, "WEIGHT").ToString()) 
                : 1.0;

            return retAKC;
        }


        private static IQueryable<AnswerKeyCampus> ConvertRowToAKCs(DataTable table)
        {
            HashSet<AnswerKeyCampus> finaldata = new HashSet<AnswerKeyCampus>();
            foreach (DataRow row in table.Rows)
            {
                AnswerKeyCampus newAKC = ConvertRowToAKC(row);
                finaldata.Add(newAKC);
            }

            return finaldata.AsQueryable();
        }

    }
}