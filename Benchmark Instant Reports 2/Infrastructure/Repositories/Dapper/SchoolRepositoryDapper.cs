using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    public class SchoolRepositoryDapper : ISchoolRepository
    {
        public School FindBySchoolID(int id)
        {
            string qs = Queries.GetSchoolByID.Replace("@id", id.ToString());
            var results = DapperHelper.DQuery(qs);
            return ConvertToSchoolQ(results);
        }


        public Entities.School FindBySchoolAbbr(string abbr)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Entities.School> FindHSCampuses()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Entities.School> FindJHCampuses()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Entities.School> FindSECCampuses()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Entities.School> FindELCampuses()
        {
            throw new NotImplementedException();
        }

        public References.Constants.SchoolType GetSchoolType(Entities.School school)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Entities.School> FindAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Entities.School> FindWhere(Func<Entities.School, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Add(Entities.School newentity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Entities.School entity)
        {
            throw new NotImplementedException();
        }




        private School ConvertToSchoolQ(IEnumerable<dynamic> results)
        {
            throw new NotImplementedException();
        }

    
    
    
    
    }
}