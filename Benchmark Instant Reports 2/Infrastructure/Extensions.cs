using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Infrastructure
{
    public static class Extensions
    {
        public static string ConvertListForQuery(this string[] s)
        {
            string returnstring = "";

            for (int i = 0; i < s.Length; i++)
            {
                string thisString = s[i].Replace("'", "''");
                returnstring = returnstring + "\'" + thisString + "\',";
            }
            returnstring = returnstring.Substring(0, returnstring.Length - 1);

            return returnstring;
        }
    }


}