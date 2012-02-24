using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    /// <summary>
    /// implementation of the repository service that provides access 
    /// to all repositories
    /// </summary>
    public class RepoService : IRepoService
    {
        public IAnswerKeyCampusRepository AnswerKeyCampusRepo { get; set; }
        public IAnswerKeyRepository AnswerKeyRepo { get; set; }
        public IRosterRepository RosterRepo { get; set; }
        public IScanRepository ScanRepo { get; set; }
        public ISchoolRepository SchoolRepo { get; set; }
        public ITestRepository TestRepo { get; set; }


        /// <summary>
        /// Constructor that defines each repository; these will be set via
        /// contructor injection from StructureMap
        /// </summary>
        /// <param name="answerkeycampusrepo">AnswerKeyCampus repository</param>
        /// <param name="answerkeyrepo">AnswerKey repository</param>
        /// <param name="rosterrepo">Roster repository</param>
        /// <param name="scanrepo">Scan repository</param>
        /// <param name="schoolrepo">School repository</param>
        /// <param name="testrepo">Test repository</param>
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
                               .Where(t => t.SecSchoolType == Constants.SchoolType.HighSchool ||
                                           t.SecSchoolType == Constants.SchoolType.AllSecondary)
                               .Select(t => t.TestID)
                               .ToList();
            else if (schType == Constants.SchoolType.JuniorHigh)
                return TestRepo.FindActiveTests()
                               .Where(t => t.SecSchoolType == Constants.SchoolType.JuniorHigh ||
                                           t.SecSchoolType == Constants.SchoolType.AllSecondary)
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



        public IList<string> GetCoursesForTest(string testid)
        {
            string customqueryraw = TestRepo.FindByTestID(testid).CustomQuery;
            string customquerynosch = customqueryraw.Replace("= @school", "is not null");
            string qs = Queries.GetCoursesForTest.Replace("@query", customquerynosch);

            var rosterwithuniquecourses = RosterRepo.ExecuteQuery(qs);

            List<string> finalData = new List<string>();
            foreach (var item in rosterwithuniquecourses)
            {
                finalData.Add(item.CourseID);
            }

            return finalData;
        }



        /// <summary>
        /// converts an integer student ID to a string with leading zeroes as needed
        /// </summary>
        /// <param name="id">student ID as an integer</param>
        /// <returns>a string with leading zeroes as needed</returns>
        public string StudentIDString(int id)
        {
            return ODAHelper.StudentIDString(id);
        }


        /// <summary>
        /// properly formats a student ID string with leading zeroes as needed
        /// </summary>
        /// <param name="id">student ID as a string; may or may not have leading zeroes</param>
        /// <returns>a string with leading zeroes as needed</returns>
        public string StudentIDString(string id)
        {
            return ODAHelper.StudentIDString(id);
        }


        /// <summary>
        /// converts a student ID in a string to an integer
        /// </summary>
        /// <param name="id">student ID as a string</param>
        /// <returns>an integer representing the student ID</returns>
        public int StudentIDInt(string id)
        {
            return ODAHelper.StudentIDInt(id);
        }
    }
}