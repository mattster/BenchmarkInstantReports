using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Helpers
{
    public class RememberHelper
    {
        public static string savedSelectedTestID(HttpRequest req)
        {
            if (req.Cookies[Constants.SavedSelectedTestIDCookieName] != null)
                return req.Cookies[Constants.SavedSelectedTestIDCookieName].Value;

            return null;
        }

        public static void savedSelectedTestID(HttpResponse resp, string testID)
        {
            resp.Cookies[Constants.SavedSelectedTestIDCookieName].Value = testID;
            resp.Cookies[Constants.SavedSelectedTestIDCookieName].Expires = DateTime.Now.AddDays(Constants.CookieDurationDays);
            resp.Cookies[Constants.SavedSelectedTestIDCookieName].Path = "/";

            return;
        }



        public static string savedSelectedCampus(HttpRequest req)
        {
            if (req.Cookies[Constants.SavedSelectedCampusCookieName] != null)
                return req.Cookies[Constants.SavedSelectedCampusCookieName].Value;

            return null;
        }

        public static void savedSelectedCampus(HttpResponse resp, string campus)
        {
            resp.Cookies[Constants.SavedSelectedCampusCookieName].Value = campus;
            resp.Cookies[Constants.SavedSelectedCampusCookieName].Expires = DateTime.Now.AddDays(Constants.CookieDurationDays);
            resp.Cookies[Constants.SavedSelectedCampusCookieName].Path = "/";

            return;
        }


        public static string[] savedSelectedTestIDs(HttpRequest req)
        {
            if (req.Cookies[Constants.SavedSelectedTestIDsCokieName] != null)
                return req.Cookies[Constants.SavedSelectedTestIDsCokieName].Value.Split(',');

            return null;
        }

        public static void savedSelectedTestIDs(HttpResponse resp, string[] tests)
        {
            resp.Cookies[Constants.SavedSelectedTestIDsCokieName].Value = string.Join(",", tests);
            resp.Cookies[Constants.SavedSelectedTestIDsCokieName].Expires = DateTime.Now.AddDays(Constants.CookieDurationDays);
            resp.Cookies[Constants.SavedSelectedTestIDsCokieName].Path = "/";

            return;
        }
    }
}