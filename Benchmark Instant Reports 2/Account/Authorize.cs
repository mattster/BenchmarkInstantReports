using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Account
{
    /// <summary>
    /// Authorization utilities
    /// </summary>
    public class Authorize
    {
        /// <summary>
        /// returns a set of School objects for which a user is authorized
        /// </summary>
        /// <param name="username">username from remember token (likely a cookie) representing the authorized campus</param>
        /// <param name="dataservice">current data service with repositories</param>
        /// <returns>an IQueryable list of School objects</returns>
        public static IQueryable<School> getAuthorizedCampusList(string username, IRepoService dataservice)
        {
            // if authorized for all campuses, return all schools
            if (username == Constants.UsernameAllCampuses)
                return dataservice.SchoolRepo.FindAll();
            
            // else return just the school for which the user is authorized
            var school = dataservice.SchoolRepo.FindBySchoolAbbr(username);
            var finalData = new HashSet<School>();
            finalData.Add(school);
            return finalData.AsQueryable();
        }

    }
}