using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class SchoolHelper
    {
        public static string ConvertCampusList(IRepoService dataservice, string campus)
        {
            // if "ALL ELEMENTARY", get a list of all elementaries
            if (campus == Constants.DispAllElementary)
            {
                var campuses = dataservice.SchoolRepo.FindELCampuses().Select(s => s.Abbr).ToArray<string>();
                return campuses.ConvertListForQuery();
            }

            // if "ALL SECONDARY", get a list of all secondary schools
            else if (campus == Constants.DispAllSecondary)
            {
                var campuses = dataservice.SchoolRepo.FindSECCampuses().Select(s => s.Abbr).ToArray<string>();
                return campuses.ConvertListForQuery();
            }

            // just return the single campus abbreviation
            else
            {
                return "\'" + campus + "\'";
            }
        }





    }
}