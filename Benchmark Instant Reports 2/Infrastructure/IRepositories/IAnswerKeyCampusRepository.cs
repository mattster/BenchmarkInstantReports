using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    public interface IAnswerKeyCampusRepository : IRepository<AnswerKeyCampus>
    {
        IQueryable<AnswerKeyCampus> FindKeyForTest(string testid, string campusabbr);
    }
}