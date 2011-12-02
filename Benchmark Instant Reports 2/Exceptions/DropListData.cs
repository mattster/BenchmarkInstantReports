using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Exceptions
{
    public static partial class ExceptionData
    {
        public static List<DropList> CampusDropLists = new List<DropList>
                {
                    new DropList("2010-12 SC IPC-M SEM 22-37", new string[] { "LHHS", "LHFC"}, new int[] { 32, 33, 34, 35, 36, 37, 38, 39, 40 }),
                    new DropList("2010-12 SC Chemistry-M SEM 22-38", new string[] { "LHHS" }, new int[] { 29, 30, 31, 32, 33, 34, 35 }),
                    new DropList("2011-08 SC Biology Unit 1 CUT 91-11", new string[] { "LHHS" }, new int[] { 32, 33, 34, 35, 36, 37, 38, 39, 40 }),
                    


                };

    }
}