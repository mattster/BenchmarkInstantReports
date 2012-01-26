using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    public class RosterRepositoryODA : IRosterRepository
    {
        public IQueryable<Roster> FindAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Roster> FindWhere(Func<Roster, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(Roster newentity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Roster entity)
        {
            throw new NotImplementedException();
        }




        private static Roster ConvertRowToRoster(DataRow row)
        {
            Roster retRoster = new Roster();
            retRoster.StudentName = row["STUDENT_NAME"].ToString();
            retRoster.StudentID = row["STUDENT_ID"].ToString();
            retRoster.LEPCode = row["LEP_CODE"].ToString();
            retRoster.SPEDFlag = row["SPECIAL_ED_FLAG"].ToString();
            retRoster.Grade = row["GRADE_LEVEL"].ToString();
            retRoster.SchoolID = row["STATE_SCHOOL_ID"].ToString();
            retRoster.TeacherNum = row["TEACHER_NBR"].ToString();
            retRoster.TeacherName = row["TEACHER_NAME"].ToString();
            retRoster.Active = row["ACTIVE"].ToString();
            retRoster.CourseID = row["LOCAL_COURSE_ID"].ToString();
            retRoster.CourseTitle = row["DISTRICT_COURSE_TITLE"].ToString();
            retRoster.Semester = row["SEMESTER"].ToString();
            retRoster.Period = row["PERIOD"].ToString();
            retRoster.HomeCampus = row["SCHOOL_ABBR"].ToString();
            retRoster.USYears = row["US_YRS"].ToString();
            retRoster.FirstYear = row["FIRST_YEAR"].ToString();
            retRoster.ModifiedFlag = row["BENCHMARK_MOD"].ToString();
            retRoster.CourseCampus = row["SCHOOL2"].ToString();

            return retRoster;
        }


        private static IQueryable<Roster> ConvertRowToRosters(DataTable table)
        {
            HashSet<Roster> finaldata = new HashSet<Roster>();
            foreach (DataRow row in table.Rows)
            {
                Roster newRoster = ConvertRowToRoster(row);
                finaldata.Add(newRoster);
            }

            return finaldata.AsQueryable();
        }

    }
}