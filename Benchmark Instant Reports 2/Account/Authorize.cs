using System.Collections.Generic;
using System.Linq;
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
        public static IQueryable<School> GetAuthorizedSchools(string username, IRepoService dataservice)
        {
            var finalData = new HashSet<School>();

            if (IsAuthorizedForAllCampuses(username))
            {
                foreach (School school in dataservice.SchoolRepo.FindHSCampuses().OrderBy(s => s.Name))
                    finalData.Add(school);
                
                finalData.Add(new School(Constants.DropDownSeparatorString, Constants.DropDownSeparatorString));

                foreach (School school in dataservice.SchoolRepo.FindJHCampuses().OrderBy(s => s.Name))
                    finalData.Add(school);
                
                finalData.Add(new School(Constants.DropDownSeparatorString, Constants.DropDownSeparatorString));

                foreach (School school in dataservice.SchoolRepo.FindELCampuses().OrderBy(s => s.Name))
                    finalData.Add(school);
               
                return finalData.AsQueryable();
            }
             
            // else return just the school for which the user is authorized
            var oneschool = dataservice.SchoolRepo.FindBySchoolAbbr(username);
            finalData.Add(oneschool);
            return finalData.AsQueryable();
        }


        /// <summary>
        /// determines if the current user is authorized for all campuses
        /// </summary>
        /// <param name="username">username to check</param>
        /// <returns>true if current user is authorized for all campuses, false otherwise</returns>
        public static bool IsAuthorizedForAllCampuses(string username)
        {
            if (username == Constants.UsernameAllCampuses)
                return true;

            return false;
        }


        /// <summary>
        /// attempts to authorize a user based on a school abbreviation and a password
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="schoolAbbr">School abbreviation of the school</param>
        /// <param name="password">user's entered password</param>
        /// <returns>true if user can be authorized, false otherwise</returns>
        public static bool AuthorizeUser(IRepoService dataservice, string schoolAbbr, string password)
        {
            if (isDistrictPassword(password))
                return true;

            else if (isPrincipalPassword(password))
                return true;

            else if (isSchoolPassword(dataservice, schoolAbbr, password))
                return true;

            return false;
        }






        /// <summary>
        /// determines if a password is valid for a school
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="schoolAbbr">School abbreviation of the school</param>
        /// <param name="password">user's entered password</param>
        /// <returns>true if password is valid, false otherwise</returns>
        private static bool isSchoolPassword(IRepoService dataservice, string schoolAbbr, string password)
        {
            School school = dataservice.SchoolRepo.FindBySchoolAbbr(schoolAbbr);
            if (password == school.Password)
                return true;

            return false;
        }

        
        /// <summary>
        /// determines if a password is the District password
        /// </summary>
        /// <param name="password">user's entered password</param>
        /// <returns>true if password is the District password, false otherwise</returns>
        private static bool isDistrictPassword(string password)
        {
            if (password == Security.DistrictPassword)
                return true;

            return false;
        }


        /// <summary>
        /// determines if a password is the generic Principal's password
        /// </summary>
        /// <param name="password">user's entered password</param>
        /// <returns>true if password is the generic Principal's password, false otherwise</returns>
        private static bool isPrincipalPassword(string password)
        {
            if (password == Security.PrincipalPassword)
                return true;

            return false;
        }




    }
}