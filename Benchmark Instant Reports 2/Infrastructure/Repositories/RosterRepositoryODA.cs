using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    /// <summary>
    /// implementation of the Roster repository using Oracle Data Access
    /// </summary>
    public class RosterRepositoryODA : IRosterRepository
    {
        /// <summary>
        /// performs a query and returns the results from the Roster
        /// </summary>
        /// <param name="qs">query string of the query to perform</param>
        /// <returns>IQueryable-Roster- data</returns>
        public IQueryable<Roster> ExecuteTestQuery(string qs)
        {
            DataSet ds = ODAHelper.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToRosters(ds.Tables[0]);
        }


        /// <summary>
        /// find Roster items for a specific student by Student ID
        /// </summary>
        /// <param name="id">student ID in a string</param>
        /// <returns>IQueryable-Roster- data</returns>
        public IQueryable<Roster> FindByStudentID(string id)
        {
            // convert id to a 6-digit string with leading zero(es) if needed
            string qs = Queries.GetRosterDataForID.Replace("@studentId", 
                ODAHelper.StudentIDString(id));
            DataSet ds = ODAHelper.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToRosters(ds.Tables[0]);
        }


        /// <summary>
        /// find all Roster items for a specific school
        /// </summary>
        /// <param name="schoolAbbr">School abbreviation of the School to use</param>
        /// <returns>IQueryable-Roster- data</returns>
        public IQueryable<Roster> FindBySchool(string schoolAbbr)
        {
            string qs = Queries.GetAbbreviatedRosterDataForSchool;
            qs = qs.Replace("@school", schoolAbbr);
            DataSet ds = ODAHelper.getDataRows(qs);
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



        /// <summary>
        /// converts a row read in from the database to a Roster object
        /// </summary>
        /// <param name="row">DataRow read in from the database</param>
        /// <returns>a Roster object with the data</returns>
        private static Roster ConvertRowToRoster(DataRow row)
        {
            Roster retRoster = new Roster();
            retRoster.StudentName = ODAHelper.GetTableValueSafely(row, "STUDENT_NAME").ToString();
            retRoster.StudentID = ODAHelper.GetTableValueSafely(row, "LOCAL_STUDENT_ID").ToString();
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


        /// <summary>
        /// converts a table of data read in from the database to an 
        /// IQueryable-Roster- list of objects
        /// </summary>
        /// <param name="table">DataTable read in from the database</param>
        /// <returns>IQueryable-Roster- list of data objects</returns>
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