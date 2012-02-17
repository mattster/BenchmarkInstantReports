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
    /// implementation of the AnswerKey repository using Oracle Data Access
    /// </summary>
    public class AnswerKeyRepositoryODA : IAnswerKeyRepository
    {
        /// <summary>
        /// returns an answer key for a specific test
        /// </summary>
        /// <param name="testid">TestID to use</param>
        /// <returns>IQueryable-AnswerKey- collection of AnswerKey data</returns>
        public IQueryable<AnswerKey> FindKeyForTest(string testid)
        {
            string qs = Queries.GetDistrictTestAnswerKey.Replace("@testId", testid);
            DataSet ds = ODAHelper.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToAKs(ds.Tables[0]);
        }


        /// <summary>
        /// retuns all data
        /// </summary>
        /// <returns>IQueryable-AnswerKey- collection of AnswerKey data</returns>
        public IQueryable<AnswerKey> FindAll()
        {
            string qs = Queries.GetAllDistrictAnswerKeys;
            DataSet ds = ODAHelper.getDataRows(qs);
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



        /// <summary>
        /// converts a row read in from the database to an AnswerKey object
        /// </summary>
        /// <param name="row">DataRow read in from the database</param>
        /// <returns>an AnswerKey object with the data</returns>
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


        /// <summary>
        /// converts a table of data read in from the database to an 
        /// IQueryable-AnswerKey list of objects
        /// </summary>
        /// <param name="table">DataTable read in from the database</param>
        /// <returns>IQueryable-AnswerKey- list of data objects</returns>
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