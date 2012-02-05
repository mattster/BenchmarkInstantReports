using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Helpers;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    public class RepoService : IRepoService
    {
        public IAnswerKeyCampusRepository AnswerKeyCampusRepo { get; set; }
        public IAnswerKeyRepository AnswerKeyRepo { get; set; }
        public IRosterRepository RosterRepo { get; set; }
        public IScanRepository ScanRepo { get; set; }
        public ISchoolRepository SchoolRepo { get; set; }
        public ITestRepository TestRepo { get; set; }

        public RepoService(IAnswerKeyCampusRepository answerkeycampusrepo, IAnswerKeyRepository answerkeyrepo,
                           IRosterRepository rosterrepo, IScanRepository scanrepo, ISchoolRepository schoolrepo,
                           ITestRepository testrepo)
        {
            AnswerKeyCampusRepo = answerkeycampusrepo;
            AnswerKeyRepo = answerkeyrepo;
            RosterRepo = rosterrepo;
            ScanRepo = scanrepo;
            SchoolRepo = schoolrepo;
            TestRepo = testrepo;
        }

        /// <summary>
        /// return a list of TestIDs that are applicable to a specific school
        /// </summary>
        /// <param name="abbr">abbreviation of school to use</param>
        /// <returns>List-string- of Test IDs</returns>
        public IList<string> GetTestIDsForSchool(string abbr)
        {
            if (abbr == Constants.DispAllElementary)
            {
                return TestRepo.FindActiveTests()
                               .Where(t => t.SchoolType == Constants.SchoolType.Elementary)
                               .Select(t => t.TestID)
                               .ToList();
            }
            else if (abbr == Constants.DispAllSecondary)
            {
                return TestRepo.FindActiveTests()
                               .Where(t => t.SchoolType == Constants.SchoolType.AllSecondary)
                               .Select(t => t.TestID)
                               .ToList();
            }

            var sch = SchoolRepo.FindBySchoolAbbr(abbr);
            var schType = SchoolRepo.GetSchoolType(sch);
            
            if (schType == Constants.SchoolType.HighSchool)
                return TestRepo.FindActiveTests()
                               .Where(t => t.SecSchoolType == Constants.SchoolType.HighSchool)
                               .Select(t => t.TestID)
                               .ToList();
            else if (schType == Constants.SchoolType.JuniorHigh)
                return TestRepo.FindActiveTests()
                               .Where(t => t.SecSchoolType == Constants.SchoolType.JuniorHigh)
                               .Select(t => t.TestID)
                               .ToList();
            else if (schType == Constants.SchoolType.Elementary)
                return TestRepo.FindActiveTests()
                               .Where(t => t.SchoolType == Constants.SchoolType.Elementary)
                               .Select(t => t.TestID)
                               .ToList();
            else
                return TestRepo.FindActiveTests().Select(t => t.TestID).ToList();
        }


    }
}