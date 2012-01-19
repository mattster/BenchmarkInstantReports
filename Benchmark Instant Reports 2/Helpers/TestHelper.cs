using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Grading;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class TestHelper
    {
        public static string ReturnRawCustomQuery(string testid)
        {
            return DBIOWorkaround.ReturnRawCustomQuery(testid);
        }

        public static bool UsesWeightedAnswers(string testid)
        {
            // get answer key for test
            List<AnswerKeyItem> theAnswerKey = AnswerKey.getDistrictAnswerKey(testid);

            // if any of the weights are != 1, return true
            List<decimal> weights = theAnswerKey.Select(k => k.Weight).Distinct().ToList();
            if (weights.Count != 1 || weights[0] != 1)
                return true;

            return false;
        }

    }
}