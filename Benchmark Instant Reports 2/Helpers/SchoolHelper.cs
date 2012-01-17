﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class SchoolHelper
    {
        public static string ConvertCampusList(string campus)
        {
            // if "ALL ELEMENTARY", get a list of all elementaries
            if (campus == "ALL Elementary")
            {
                return DBIOWorkaround.ReturnElemAbbrList().ConvertListForQuery();
            }

            // if "ALL SECONDARY", get a list of all secondary schools
            else if (campus == "ALL Secondary")
            {
                return DBIOWorkaround.ReturnSecAbbrList().ConvertListForQuery();
            }

            // just return the single campus abbreviation
            else
            {
                return "\'" + campus + "\'";
            }
        }





    }
}