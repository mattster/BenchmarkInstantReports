using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    public interface IScanRepository : IRepository<Scan>
    {
        IQueryable<Scan> FindScansForTestCampus(string testid, string abbr);
    }
}