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
    /// implementation of the School repository using Oracle Data Access
    /// </summary>
    public class SchoolRepositoryODA : ISchoolRepository
    {
        /// <summary>
        /// finds a School by its numeric ID
        /// </summary>
        /// <param name="id">integer ID of the school to find</param>
        /// <returns>School object of the found School</returns>
        public School FindBySchoolID(int id)
        {
            string qs = Queries.GetSchoolByID.Replace("@id", id.ToString());
            DataSet ds = ODAHelper.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToSchool(ds.Tables[0].Rows[0]);
        }


        /// <summary>
        /// finds a School by its abbreviation
        /// </summary>
        /// <param name="abbr">string abbreviation of the school to find</param>
        /// <returns>School object of the found School</returns>
        public School FindBySchoolAbbr(string abbr)
        {
            string qs = Queries.GetSchoolByAbbr.Replace("@abbr", abbr);
            DataSet ds = ODAHelper.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToSchool(ds.Tables[0].Rows[0]);
        }


        /// <summary>
        /// finds all High Schools
        /// </summary>
        /// <returns>IQueryable-School- collection of data objects</returns>
        public IQueryable<School> FindHSCampuses()
        {
            string qs = Queries.GetHSCampuses;
            DataSet ds = ODAHelper.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToSchools(ds.Tables[0]);
        }


        /// <summary>
        /// finds all Junior High Schools
        /// </summary>
        /// <returns>IQueryable-School- collection of data objects</returns>
        public IQueryable<School> FindJHCampuses()
        {
            string qs = Queries.GetJHCampuses;
            DataSet ds = ODAHelper.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToSchools(ds.Tables[0]);
        }


        /// <summary>
        /// finds all Secondary (High School + Junior High) Schools
        /// </summary>
        /// <returns>IQueryable-School- collection of data objects</returns>
        public IQueryable<School> FindSECCampuses()
        {
            string qs = Queries.GetSECCampuses;
            DataSet ds = ODAHelper.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToSchools(ds.Tables[0]);
        }


        /// <summary>
        /// finds all Elementary Schools
        /// </summary>
        /// <returns>IQueryable-School- collection of data objects</returns>
        public IQueryable<Entities.School> FindELCampuses()
        {
            string qs = Queries.GetELCampuses;
            DataSet ds = ODAHelper.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToSchools(ds.Tables[0]);
        }


        /// <summary>
        /// finds all Schools
        /// </summary>
        /// <returns>IQueryable-School- collection of data objects</returns>
        public IQueryable<Entities.School> FindAll()
        {
            string qs = Queries.GetAllSchools;
            DataSet ds = ODAHelper.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToSchools(ds.Tables[0]);
        }


        /// <summary>
        /// finds Schools based on a predicate Linq query
        /// </summary>
        /// <param name="predicate">Linq query</param>
        /// <returns>IQueryable-School- collection of data objects</returns>
        public IQueryable<Entities.School> FindWhere(Func<Entities.School, bool> predicate)
        {
            return FindAll().Where(predicate).AsQueryable();
        }


        /// <summary>
        /// returns the type of the specified School
        /// </summary>
        /// <param name="school">School object</param>
        /// <returns>Constants.SchoolType (Elem., JH., HS., All)</returns>
        public Constants.SchoolType GetSchoolType(School school)
        {
            if (school.ID == 6)
                return Constants.SchoolType.All;
            else if (school.ID < 10 || school.ID == 52)
                return Constants.SchoolType.HighSchool;
            else if (school.ID >= 10 && school.ID < 100)
                return Constants.SchoolType.JuniorHigh;
            else if (school.ID >= 100)
                return Constants.SchoolType.Elementary;

            return Constants.SchoolType.All;
        }




        public void Add(Entities.School newentity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Entities.School entity)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// converts a row read in from the database to a School object
        /// </summary>
        /// <param name="row">DataRow read in from the database</param>
        /// <returns>a School object with the data</returns>
        private static School ConvertRowToSchool(DataRow row)
        {
            School retSchool = new School();
            retSchool.ID = (ODAHelper.GetTableValueSafely(row, "SCHOOLID").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "SCHOOLID").ToString()) 
                : 0;
            retSchool.Name = ODAHelper.GetTableValueSafely(row, "SCHOOLNAME").ToString();
            retSchool.Password = ODAHelper.GetTableValueSafely(row, "SCHOOLPASSWORD").ToString();
            retSchool.Principal = ODAHelper.GetTableValueSafely(row, "PRINCIPAL").ToString();
            retSchool.Area = ODAHelper.GetTableValueSafely(row, "AREA").ToString();
            retSchool.Loc = (ODAHelper.GetTableValueSafely(row, "LOC").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "LOC").ToString()) 
                : 0;
            retSchool.Phone = ODAHelper.GetTableValueSafely(row, "PHONE").ToString();
            retSchool.Username = ODAHelper.GetTableValueSafely(row, "USERNAME").ToString();
            retSchool.Abbr = ODAHelper.GetTableValueSafely(row, "SCHOOL_ABBR").ToString();
            retSchool.Cluster = (ODAHelper.GetTableValueSafely(row, "CLUSTERNUM").ToString() != "") 
                ? Convert.ToInt32(ODAHelper.GetTableValueSafely(row, "CLUSTERNUM").ToString()) 
                : 0;

            return retSchool;
        }


        /// <summary>
        /// converts a table of data read in from the database to an 
        /// IQueryable-School- list of objects
        /// </summary>
        /// <param name="table">DataTable read in from the database</param>
        /// <returns>IQueryable-School- list of collection of data objects</returns>
        private static IQueryable<School> ConvertTableToSchools(DataTable table)
        {
            HashSet<School> finaldata = new HashSet<School>();
            foreach (DataRow row in table.Rows)
            {
                School newSchool = ConvertRowToSchool(row);
                finaldata.Add(newSchool);
            }

            return finaldata.AsQueryable();
        }

    }
}