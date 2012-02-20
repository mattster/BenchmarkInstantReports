using System.Collections.Generic;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    public class MatCheckRepHelper
    {
        /// <summary>
        /// simply returns a one-member data structure containing the report title;
        /// required to use as a datasource in the Materials Checklist report
        /// </summary>
        /// <param name="title">the title of the report</param>
        /// <returns>a one-member list of ItemInfo-string- containing the title</returns>
        public static List<ItemInfo<string>>  GetReportTitle(string title)
        {
            List<ItemInfo<string>> thelist = new List<ItemInfo<string>>();
            thelist.Add(new ItemInfo<string>(1, title));
            return thelist;
        }
    }
}