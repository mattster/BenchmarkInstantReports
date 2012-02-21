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
            return ConvertPreslugToRosterQ(results);
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

                newRoster.StudentName = item.STUDENT_NAME;
                newRoster.StudentID = item.LOCAL_STUDENT_ID;
                newRoster.LEPCode = item.LEP_CODE;
                newRoster.SPEDFlag = item.SPECIAL_ED_FLAG;
                newRoster.Grade = item.GRADE_LEVEL;
                newRoster.SchoolID = item.STATE_SCHOOL_ID;
                newRoster.TeacherNum = item.TEACHER_NBR;
                newRoster.TeacherName = item.TEACHER_NAME;
                newRoster.Active = item.ACTIVE.ToString();
                newRoster.CourseID = item.LOCAL_COURSE_ID;
                newRoster.CourseTitle = item.DISTRICT_COURSE_TITLE;
                newRoster.Semester = item.SEMESTER;
                newRoster.Period = item.PERIOD;
                newRoster.HomeCampus = item.SCHOOL_ABBR;
                newRoster.USYears = item.US_YRS;
                newRoster.FirstYear = item.FIRST_YEAR;
                newRoster.ModifiedFlag = item.BENCHMARK_MOD;
                newRoster.CourseCampus = item.SCHOOL2; 

                finalData.Add(newRoster);
            }

            return finalData.AsQueryable();
        }



        private IQueryable<Roster> ConvertPreslugToRosterQ(IEnumerable<dynamic> rawdata)
        {
            HashSet<Roster> finalData = new HashSet<Roster>();

            foreach (var item in rawdata)
            {
                Roster newRoster = new Roster();

                newRoster.StudentName = item.STUDENT_NAME;
                newRoster.StudentID = item.LOCAL_STUDENT_ID;
                newRoster.Grade = item.GRADE_LEVEL;
                newRoster.SchoolID = item.STATE_SCHOOL_ID;
                newRoster.TeacherNum = item.TEACHER_NBR;
                newRoster.TeacherName = item.TEACHER_NAME;
                newRoster.CourseID = item.LOCAL_COURSE_ID;
                newRoster.CourseTitle = item.DISTRICT_COURSE_TITLE;
                newRoster.Semester = item.SEMESTER;
                newRoster.Period = item.PERIOD;

                finalData.Add(newRoster);
            }

            return finalData.AsQueryable();
        }
    }
}