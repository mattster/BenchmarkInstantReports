using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    /// <summary>
    /// interface definition for the Scan repository
    /// </summary>
    public interface IScanRepository : IRepository<Scan>
    {
        IQueryable<Scan> FindScansForTestCampus(string testid, string abbr);
    }
}