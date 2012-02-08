using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    /// <summary>
    /// interface definition for the Test repository
    /// </summary>
    public interface ITestRepository : IRepository<Test>
    {
        Test FindByTestID(string testid);
        IQueryable<Test> FindActiveTests();
    }
}