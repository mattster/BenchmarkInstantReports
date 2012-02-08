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
    /// implementation of the AnswerKeyCampus repository using Oracle Data Access
    /// </summary>
    public class AnswerKeyCampusRepositoryODA : IAnswerKeyCampusRepository
    {
        /// <summary>
        /// returns an answer key for a specific test and school
        /// </summary>
        /// <param name="testid">TestID to use</param>
        /// <param name="schoolAbbr">School abbreviation of the school to use</param>
        /// <returns>IQueryable-AnswerKeyCampus- collection of AnswerKeyCampus data</returns>
        public IQueryable<AnswerKeyCampus> FindKeyForTest(string testid, string schoolAbbr)
        {
            string qs = Queries.GetCampusTestAnswerKey.Replace("@testId", testid);
            qs = qs.Replace("@schoolAbbr", schoolAbbr);
            DataSet ds = ODAHelper.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToAKCs(ds.Tables[0]);
        }


        /// <summary>
        /// retuns all data
        /// </summary>
        /// <returns>IQueryable-AnswerKeyCampus- collection of AnswerKeyCampus data</returns>
        public IQueryable<AnswerKeyCampus> FindAll()
        {
            string qs = Queries.GetAllCampusAnswerKeys;
            DataSet ds = ODAHelper.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToAKCs(ds.Tables[0]);
        }



        public IQueryable<AnswerKeyCampus> FindWhere(Func<AnswerKeyCampus, bool> predicate)
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




        /// <summary>
        /// converts a row read in from the database to an AnswerKeyCampus object
        /// </summary>
        /// <param name="row">DataRow read in from the database</param>
        /// <returns>an AnswerKeyCampus object with the data</returns>
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

        /// <summary>
        /// converts a table of data read in from the database to an 
        /// IQueryable-AnswerKeyCampus list of objects
        /// </summary>
        /// <param name="table">DataTable read in from the database</param>
        /// <returns>IQueryable-AnswerKeyCampus- list of data objects</returns>
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