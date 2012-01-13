using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace Benchmark_Instant_Reports_2.Interfaces.EF
{
    public class ReportStatsEntities : ObjectContext
    {
        public ReportStatsEntities() : base("name=ReportStatsEntities") 
        {
            ContextOptions.LazyLoadingEnabled = true;
        }

        public ObjectSet<BenchmarkStat> BenchmarkStats
        {
            get { return CreateObjectSet<BenchmarkStat>(); }
        }

    }
}