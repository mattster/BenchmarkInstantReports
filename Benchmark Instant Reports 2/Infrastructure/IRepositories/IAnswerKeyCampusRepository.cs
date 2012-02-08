using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    /// <summary>
    /// interface definition for the AnswerKeyCampus repository
    /// </summary>
    public interface IAnswerKeyCampusRepository : IRepository<AnswerKeyCampus>
    {
        IQueryable<AnswerKeyCampus> FindKeyForTest(string testid, string campusabbr);
    }
}