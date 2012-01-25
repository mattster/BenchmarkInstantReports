using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    public interface IRepoService
    {
        public ISchoolRepository SchoolRepo { get; set; }


    }
}