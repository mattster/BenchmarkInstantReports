﻿using System;
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
    public class AnswerKeyRepositoryODA : IAnswerKeyRepository
    {
        public IQueryable<AnswerKey> FindKeyForTest(string testid)
        {
            string qs = Queries.GetDistrictTestAnswerKey.Replace("@testId", testid);
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToAKs(ds.Tables[0]);

        }

        public IQueryable<AnswerKey> FindAll()
        {
            string qs = Queries.GetAllDistrictAnswerKeys;
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToAKs(ds.Tables[0]);

        }

        public IQueryable<AnswerKey> FindWhere(Func<AnswerKey, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(AnswerKey newentity)
        {
            throw new NotImplementedException();
        }

        public void Remove(AnswerKey entity)
        {
            throw new NotImplementedException();
        }



        private static AnswerKey ConvertRowToAK(DataRow row)
        {
            AnswerKey retAK = new AnswerKey();
            retAK.TestID = ODAHelper.GetTableValueSafely(row, "TEST_ID").ToString();
            retAK.ItemNum = (ODAHelper.GetTableValueSafely(row, "ITEM_NUM").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "ITEM_NUM").ToString()) 
                : 0;
            retAK.Answer = ODAHelper.GetTableValueSafely(row, "ANSWER").ToString();
            retAK.Objective = (ODAHelper.GetTableValueSafely(row, "OBJECTIVE").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "OBJECTIVE").ToString()) 
                : 0;
            retAK.TEKS = ODAHelper.GetTableValueSafely(row, "TEKS").ToString();
            retAK.Weight = (ODAHelper.GetTableValueSafely(row, "WEIGHT").ToString() != "") 
                ? Convert.ToDouble(ODAHelper.GetTableValueSafely(row, "WEIGHT").ToString()) 
                : 1.0;

            return retAK;
        }


        private static IQueryable<AnswerKey> ConvertRowToAKs(DataTable table)
        {
            HashSet<AnswerKey> finaldata = new HashSet<AnswerKey>();
            foreach (DataRow row in table.Rows)
            {
                AnswerKey newAK = ConvertRowToAK(row);
                finaldata.Add(newAK);
            }

            return finaldata.AsQueryable();
        }

    }
}