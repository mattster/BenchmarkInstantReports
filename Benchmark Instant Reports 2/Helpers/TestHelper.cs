using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class TestHelper
    {
        public static string ReturnRawCustomQuery(string testid)
        {
            return DBIOWorkaround.ReturnRawCustomQuery(testid);
        }

    }
}