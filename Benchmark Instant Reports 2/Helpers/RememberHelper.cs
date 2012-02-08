using System;
using System.Web;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class RememberHelper
    {
        /// <summary>
        /// gets the TestID that was saved (via a cookie) as a selected test
        /// </summary>
        /// <param name="req">HttpRequest from the user, used to access cookie data</param>
        /// <returns>a saved value if there is one, null otherwise</returns>
        public static string SavedSelectedTestID(HttpRequest req)
        {
            if (req.Cookies[Constants.SavedSelectedTestIDCookieName] != null)
                return req.Cookies[Constants.SavedSelectedTestIDCookieName].Value;

            return null;
        }


        /// <summary>
        /// save a TestID (via a cookie) as a selected test
        /// </summary>
        /// <param name="resp">HttpResponse send back to the user with the updated cookie data</param>
        /// <param name="testID">TestID to save</param>
        public static void SaveSelectedTestID(HttpResponse resp, string testID)
        {
            resp.Cookies[Constants.SavedSelectedTestIDCookieName].Value = testID;
            resp.Cookies[Constants.SavedSelectedTestIDCookieName].Expires = DateTime.Now.AddDays(Constants.CookieDurationDays);
            resp.Cookies[Constants.SavedSelectedTestIDCookieName].Path = "/";

            return;
        }


        /// <summary>
        /// gets a list of TestIDs that were saved (via a cookie) as selected tests
        /// </summary>
        /// <param name="req">HttpRequest from the user, used to access cookie data</param>
        /// <returns>an array of saved values if there are any, null otherwise</returns>
        public static string[] SavedSelectedTestIDs(HttpRequest req)
        {
            if (req.Cookies[Constants.SavedSelectedTestIDsCokieName] != null)
                return req.Cookies[Constants.SavedSelectedTestIDsCokieName].Value.Split(',');

            return null;
        }


        /// <summary>
        /// saves a list of TestIDs (via a cookie) as selected tests
        /// </summary>
        /// <param name="resp">HttpResponse send back to the user with the updated cookie data</param>
        /// <param name="testids">array of TestIDs to save</param>
        public static void SavedSelectedTestIDs(HttpResponse resp, string[] testids)
        {
            resp.Cookies[Constants.SavedSelectedTestIDsCokieName].Value = string.Join(",", testids);
            resp.Cookies[Constants.SavedSelectedTestIDsCokieName].Expires = DateTime.Now.AddDays(Constants.CookieDurationDays);
            resp.Cookies[Constants.SavedSelectedTestIDsCokieName].Path = "/";

            return;
        }


        /// <summary>
        /// gets the School abbreviation that was saved (via a cookie) as a selected school
        /// </summary>
        /// <param name="req">HttpRequest from the user, used to access cookie data</param>
        /// <returns>a saved value if there is one, null otherwise</returns>
        public static string SavedSelectedCampus(HttpRequest req)
        {
            if (req.Cookies[Constants.SavedSelectedCampusCookieName] != null)
                return req.Cookies[Constants.SavedSelectedCampusCookieName].Value;

            return null;
        }


        /// <summary>
        /// save a School abbreviation (via a cookie) as a selected school
        /// </summary>
        /// <param name="resp">HttpResponse send back to the user with the updated cookie data</param>
        /// <param name="schoolAbbr">School abbreviation to save</param>
        public static void SaveSelectedCampus(HttpResponse resp, string schoolAbbr)
        {
            resp.Cookies[Constants.SavedSelectedCampusCookieName].Value = schoolAbbr;
            resp.Cookies[Constants.SavedSelectedCampusCookieName].Expires = DateTime.Now.AddDays(Constants.CookieDurationDays);
            resp.Cookies[Constants.SavedSelectedCampusCookieName].Path = "/";

            return;
        }


    }
}