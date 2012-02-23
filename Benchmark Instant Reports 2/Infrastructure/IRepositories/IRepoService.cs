using System.Collections.Generic;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    /// <summary>
    /// interface definition for the repository service that will access
    /// all the other repositories
    /// </summary>
    public interface IRepoService
    {
        IAnswerKeyCampusRepository AnswerKeyCampusRepo { get; set; }
        IAnswerKeyRepository AnswerKeyRepo { get; set; }
        IRosterRepository RosterRepo { get; set; }
        IScanRepository ScanRepo { get; set; }
        ISchoolRepository SchoolRepo { get; set; }
        ITestRepository TestRepo { get; set; }

        IList<string> GetTestIDsForSchool(string abbr);
        IList<string> GetCoursesForTest(string testid);

        string StudentIDString(int id);
        string StudentIDString(string id);
        int StudentIDInt(string id);
    }
}