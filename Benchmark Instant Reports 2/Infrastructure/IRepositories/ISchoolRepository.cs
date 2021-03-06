﻿using System.Linq;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    /// <summary>
    /// interface definition for the School repository
    /// </summary>
    public interface ISchoolRepository : IRepository<School>
    {
        School FindBySchoolID(int id);
        School FindBySchoolAbbr(string abbr);

        IQueryable<School> FindHSCampuses();
        IQueryable<School> FindJHCampuses();
        IQueryable<School> FindSECCampuses();
        IQueryable<School> FindELCampuses();

        Constants.SchoolType GetSchoolType(School school);

    }
}