using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    public class RosterRepositoryODA : IRosterRepository
    {
        public IQueryable<Roster> ExecuteTestQuery(string qs)
        {
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToRosters(ds.Tables[0]);
        }


        public IQueryable<Roster> FindByStudentID(string id)
        {
            id = string.Format("{0,6:D6}", id);
            string qs = Queries.GetRosterDataForID.Replace("@studentId", id);
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToRosters(ds.Tables[0]);
        }




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
            retRoster.StudentName = ODAHelper.GetTableValueSafely(row, "STUDENT_NAME").ToString();
            retRoster.StudentID = ODAHelper.GetTableValueSafely(row, "STUDENT_ID").ToString();
            retRoster.LEPCode = ODAHelper.GetTableValueSafely(row, "LEP_CODE").ToString();
            retRoster.SPEDFlag = ODAHelper.GetTableValueSafely(row, "SPECIAL_ED_FLAG").ToString();
            retRoster.Grade = ODAHelper.GetTableValueSafely(row, "GRADE_LEVEL").ToString();
            retRoster.SchoolID = ODAHelper.GetTableValueSafely(row, "STATE_SCHOOL_ID").ToString();
            retRoster.TeacherNum = ODAHelper.GetTableValueSafely(row, "TEACHER_NBR").ToString();
            retRoster.TeacherName = ODAHelper.GetTableValueSafely(row, "TEACHER_NAME").ToString();
            retRoster.Active = ODAHelper.GetTableValueSafely(row, "ACTIVE").ToString();
            retRoster.CourseID = ODAHelper.GetTableValueSafely(row, "LOCAL_COURSE_ID").ToString();
            retRoster.CourseTitle = ODAHelper.GetTableValueSafely(row, "DISTRICT_COURSE_TITLE").ToString();
            retRoster.Semester = ODAHelper.GetTableValueSafely(row, "SEMESTER").ToString();
            retRoster.Period = ODAHelper.GetTableValueSafely(row, "PERIOD").ToString();
            retRoster.HomeCampus = ODAHelper.GetTableValueSafely(row, "SCHOOL_ABBR").ToString();
            retRoster.USYears = ODAHelper.GetTableValueSafely(row, "US_YRS").ToString();
            retRoster.FirstYear = ODAHelper.GetTableValueSafely(row, "FIRST_YEAR").ToString();
            retRoster.ModifiedFlag = ODAHelper.GetTableValueSafely(row, "BENCHMARK_MOD").ToString();
            retRoster.CourseCampus = ODAHelper.GetTableValueSafely(row, "SCHOOL2").ToString();

            return retRoster;
        }


        private static IQueryable<Roster> ConvertTableToRosters(DataTable table)
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