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
    public class SchoolRepositoryODA : ISchoolRepository
    {
        public School FindBySchoolID(int id)
        {
            string qs = Queries.GetSchoolByID.Replace("@id", id.ToString());
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToSchool(ds.Tables[0].Rows[0]);
        }

        public School FindBySchoolAbbr(string abbr)
        {
            string qs = Queries.GetSchoolByAbbr.Replace("@abbr", abbr);
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertRowToSchool(ds.Tables[0].Rows[0]);
        }

        public IQueryable<School> FindHSCampuses()
        {
            string qs = Queries.GetHSCampuses;
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToSchools(ds.Tables[0]);
        }

        public IQueryable<School> FindJHCampuses()
        {
            string qs = Queries.GetJHCampuses;
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToSchools(ds.Tables[0]);
        }

        public IQueryable<Entities.School> FindELCampuses()
        {
            string qs = Queries.GetELCampuses;
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToSchools(ds.Tables[0]);
        }

        public IQueryable<Entities.School> FindAll()
        {
            string qs = Queries.GetAllSchools;
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            return ConvertTableToSchools(ds.Tables[0]);
            throw new NotImplementedException();
        }

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





        private static School ConvertRowToSchool(DataRow row)
        {
            School retSchool = new School();
            retSchool.ID = (row["SCHOOLID"].ToString() != "") ? Convert.ToInt32(row["SCHOOLID"].ToString()) : 0;
            retSchool.Name = row["SCHOOLNAME"].ToString();
            retSchool.Password = row["SCHOOLPASSWORD"].ToString();
            retSchool.Principal = row["PRINCIPAL"].ToString();
            retSchool.Area = row["AREA"].ToString();
            retSchool.Loc = (row["LOC"].ToString() != "") ? Convert.ToInt32(row["LOC"].ToString()) : 0;
            retSchool.Phone = row["PHONE"].ToString();
            retSchool.Username = row["USERNAME"].ToString();
            retSchool.Abbr = row["SCHOOL_ABBR"].ToString();
            var mythis = row["CLUSTERNUM"].ToString();
            retSchool.Cluster = (row["CLUSTERNUM"].ToString() != "") ? Convert.ToInt32(row["CLUSTERNUM"].ToString()) : 0;

            return retSchool;
        }


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