using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.References
{
    public static class Queries
    {


        public static string queryGetTestTemplate =
        "select test_template " +
        "from " + DatabaseDefn.dbTestDefn + " " +
        "where test_id = \'@testId\'";


    }
}