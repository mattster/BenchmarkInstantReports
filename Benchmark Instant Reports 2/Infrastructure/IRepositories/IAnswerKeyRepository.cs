using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    /// <summary>
    /// interface definition for the AnswerKey repository
    /// </summary>
    public interface IAnswerKeyRepository : IRepository<AnswerKey>
    {
        IQueryable<AnswerKey> FindKeyForTest(string testid);
        
    }
}