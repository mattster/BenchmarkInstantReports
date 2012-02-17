using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.References;



namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    public class RosterRepositoryDapper : IRosterRepository
    {
        public IQueryable<Roster> ExecuteTestQuery(string qs)
        {
            var results = DapperHelper.DQuery(qs);
            return ConvertToRosterQ(results);
        }



        public IQueryable<Roster> FindByStudentID(string id)
        {
            string qs = Queries.GetRosterDataForID.Replace("@studentId",
                ODAHelper.StudentIDString(id));
            var results = DapperHelper.DQuery(qs);
            return ConvertToRosterQ(results);
        }



        public System.Linq.IQueryable<Roster> FindBySchool(string schoolAbbr)
        {
            string qs = Queries.GetAbbreviatedRosterDataForSchool;
            qs = qs.Replace("@school", schoolAbbr);
            var results = DapperHelper.DQuery(qs);
            return ConvertToRosterQ(results);
        }



        public System.Linq.IQueryable<Roster> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public System.Linq.IQueryable<Roster> FindWhere(System.Func<Roster, bool> predicate)
        {
            throw new System.NotImplementedException();
        }

        public void Add(Roster newentity)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(Roster entity)
        {
            throw new System.NotImplementedException();
        }






        private IQueryable<Roster> ConvertToRosterQ(IEnumerable<dynamic> rawdata)
        {
            HashSet<Roster> finalData = new HashSet<Roster>();
            
            foreach (var item in rawdata)
            {
                Roster newRoster = new Roster();
                DapperHelper.AssignPropertySafely(newRoster.StudentName, item, "STUDENT_NAME");
                DapperHelper.AssignPropertySafely(newRoster.StudentID, item, "STUDENT_ID");
                DapperHelper.AssignPropertySafely(newRoster.LEPCode, item, "LEP_CODE");
                DapperHelper.AssignPropertySafely(newRoster.SPEDFlag, item, "SPECIAL_ED_FLAG");
                DapperHelper.AssignPropertySafely(newRoster.Grade, item, "GRADE_LEVEL");
                DapperHelper.AssignPropertySafely(newRoster.SchoolID, item, "STATE_SCHOOL_ID");
                DapperHelper.AssignPropertySafely(newRoster.TeacherNum, item, "TEACHER_NBR");
                DapperHelper.AssignPropertySafely(newRoster.TeacherName, item, "TEACHER_NAME");
                DapperHelper.AssignPropertySafely(newRoster.Active, item, "ACTIVE");
                DapperHelper.AssignPropertySafely(newRoster.CourseID, item, "LOCAL_COURSE_ID");
                DapperHelper.AssignPropertySafely(newRoster.CourseTitle, item, "DISTRICT_COURSE_TITLE");
                DapperHelper.AssignPropertySafely(newRoster.Semester, item, "SEMESTER");
                DapperHelper.AssignPropertySafely(newRoster.Period, item, "PERIOD");
                DapperHelper.AssignPropertySafely(newRoster.HomeCampus, item, "SCHOOL_ABBR");
                DapperHelper.AssignPropertySafely(newRoster.USYears, item, "US_YRS"); 
                DapperHelper.AssignPropertySafely(newRoster.FirstYear, item, "FIRST_YEAR");
                DapperHelper.AssignPropertySafely(newRoster.ModifiedFlag, item, "BENCHMARK_MOD");
                DapperHelper.AssignPropertySafely(newRoster.CourseCampus, item, "SCHOOL2");
                newRoster.CourseCampus = item.SCHOOL2;
                finalData.Add(newRoster);
            }

            return finalData.AsQueryable();
        }


    }
}