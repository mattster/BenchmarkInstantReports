using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using System.Text.RegularExpressions;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class TestHelper
    {
        ///// <summary>
        ///// find a group of Tests that are in the same "group" as the specified test
        ///// </summary>
        ///// <param name="dataservice">IRepoService access to the data</param>
        ///// <param name="testid">Test ID to find related tests for</param>
        ///// <returns>IQueryable-Test- list of Test entity items</returns>
        //public static IQueryable<Test> FindRelatedTests(IRepoService dataservice, string testid)
        //{
        //    MatchCollection mainTestMatches = Regex.Matches(testid, Constants.TestIDRegEx);
        //    string mainTestMatchYr = mainTestMatches[1].Value;
        //    string mainTestMatchMonth = mainTestMatches[2].Value;
        //    string mainTestMatchWindow = mainTestMatches[6].Value;

        //    var activeTests = dataservice.TestRepo.FindActiveTests();
        //    HashSet<Test> finalData = new HashSet<Test>();
        //    foreach (Test test in activeTests)
        //    {
        //        MatchCollection matches = Regex.Matches(test.TestID, Constants.TestIDRegEx);
        //        // look for the same year-month and test window
        //        if (matches[1].Value == mainTestMatchYr &&
        //            matches[2].Value == mainTestMatchMonth &&
        //            matches[6].Value == mainTestMatchWindow)
        //        {
        //            finalData.Add(test);
        //        }
        //    }

        //    return finalData.AsQueryable();
        //}



        // TODO
        public static bool UsesWeightedAnswers(IRepoService dataservice, string testid)
        {
            // get answer key for test
            //List<AnswerKeyItem> theAnswerKey = AnswerKey.getDistrictAnswerKey(testid);

            //// if any of the weights are != 1, return true
            //List<decimal> weights = theAnswerKey.Select(k => k.Weight).Distinct().ToList();
            //if (weights.Count != 1 || weights[0] != 1)
            //    return true;

            return false;
        }

    }
}