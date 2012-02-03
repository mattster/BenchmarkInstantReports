using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    public interface ITestRepository : IRepository<Test>
    {
        Test FindByTestID(string testid);
        IQueryable<Test> FindActiveTests();
    }
}