using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;

namespace Benchmark_Instant_Reports_2.Infrastructure.Repositories
{
    public class SchoolRepositoryODA : ISchoolRepository
    {
        public Entities.School FindBySchoolID(int id)
        {
            throw new NotImplementedException();
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

        public IQueryable<Entities.School> FindELCampuses()
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
    }
}