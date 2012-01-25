using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    public class RepoService : IRepoService
    {
        public ISchoolRepository SchoolRepo
        {
            get;
            set;
        }
    }
}