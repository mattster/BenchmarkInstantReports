using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    public interface IRepoService
    {
        IAnswerKeyCampusRepository AnswerKeyCampusRepo { get; set; }
        IAnswerKeyRepository AnswerKeyRepo { get; set; }
        IRosterRepository RosterRepo { get; set; }
        IScanRepository ScanRepo { get; set; }
        ISchoolRepository SchoolRepo { get; set; }
        ITestRepository TestRepo { get; set; }

        IList<string> GetTestIDsForSchool(string abbr);
    }
}