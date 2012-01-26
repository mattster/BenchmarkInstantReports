using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    public interface IAnswerKeyRepository : IRepository<AnswerKey>
    {
        IQueryable<AnswerKey> FindKeyForTest(string testid);
        
    }
}