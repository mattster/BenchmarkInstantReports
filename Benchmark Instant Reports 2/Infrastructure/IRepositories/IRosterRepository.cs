using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    /// <summary>
    /// interface definition for the Roster repository
    /// </summary>
    public interface IRosterRepository : IRepository<Roster>
    {
        IQueryable<Roster> ExecuteQuery(string qs);
        IQueryable<Roster> FindByStudentID(string id);
        IQueryable<Roster> FindBySchool(string schoolAbbr);
    }
}