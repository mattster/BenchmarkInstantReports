using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using System.Text.RegularExpressions;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class TestHelper
    {
        /// <summary>
        /// determines if a test has weighted answers, i.e. uses a weight factor other than 1
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="test">Test object of the test to use</param>
        /// <returns>true if any item weights are other than 1, false if all items are weighted as 1.0</returns>
        public static bool UsesWeightedAnswers(IRepoService dataservice, Test test)
        {
            // get answer key for test
            AnswerKeyItemData theAnswerKey = AnswerKeyHelper.GetDistrictAnswerKeyItems(dataservice, test);

            // if any of the weights are != 1, return true
            List<double> weights = theAnswerKey.GetItems().Select(k => k.Weight).Distinct().ToList();
            if (weights.Count != 1 || weights[0] != 1)
                return true;

            return false;
        }


        /// <summary>
        /// returns the semester as a string applicable for a test
        /// </summary>
        /// <param name="test">Test to use</param>
        /// <returns>semester for the test, as a string</returns>
        public static string SemesterForTest(Test test)
        {
            if (test.TestMonth >= 1 && test.TestMonth <= 6)
                return "2";

            return "1";
        }
    }
}