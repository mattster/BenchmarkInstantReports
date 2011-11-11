using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
// security and authentication for RISD campuses
//
// matt hollingsworth
// oct 2010
//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *


namespace Benchmark_Instant_Reports_2
{
    public static class CampusSecurity
    {
        private static int authActionAuthOneCampus = 1;                      // authorize a user for 1 campus
        private static int authActionDeauthOneCampus = -1;                   // deauthorize a user for 1 campus
        private static int authActionAuthAll = 99;                           // authorize a user for all campuses
        private static int authActionDeauthAll = 0;                          // de-authorize a user for all campuses
        private static string districtPwd = "28774";                         // district password
        private static string principalPwd = "40996";                       // principal password
        private static string authErrorString = "xxxxxxxxxxxxxxxxxxxxERRORxxxxxxxxxxxxxxxxxxxx";
        public static string authcookiename = "campusauthlist";            // name of the cookie
        public static string authAllCampusesValue = "All";
        public static int cookieDurationDays = 5;
        private static string[] authForTeacherComparison = { "BHS" };       // campuses that can see teacher comparison on item analysis
        private static string addtlDigitForPrincipalPwd = "9";                   // digit added to school password to auth principals


        public static bool isAuthorized(string campus, HttpRequest req)
        {
            return isAuthorizedForCampus(campus, req);
        }

        public static bool isAuthorizedAsAdmin(HttpRequest req)
        {
            if (req.Cookies[authcookiename] != null)
                if (req.Cookies[authcookiename].Value == authAllCampusesValue)
                    return true;
            return false;
        }

        public static bool isAuthorizedForTeacherComparison(HttpRequest req)
        {
            // yes if logged in as an admin
            if (isAuthorizedAsAdmin(req))
                return true;

            // yes if logged in as a school in the allowed list
            foreach (string campus in authForTeacherComparison)
            {
                if (isAuthorizedFor(req) == campus)
                    return true;
            }

            return false;
        }

        public static bool isAuthorizedForCampusRepMenu(HttpRequest req)
        {
            // yes if logged in as an admin
            if (isAuthorizedAsAdmin(req))
                return true;

            // yes if logged in as an elementary campus
            foreach (string campus in birIF.getElemAbbrList())
            {
                if (isAuthorizedFor(req) == campus)
                    return true;
            }

            return false;
        }

        public static string isAuthorizedFor(HttpRequest req)
        {
            if (req.Cookies[authcookiename] != null)
                if (req.Cookies[authcookiename].Value != null)
                    return req.Cookies[authcookiename].Value;

            return "none";
        }

        public static void deAuthorize(HttpResponse resp)
        {
            setUserAuthForCampus(authActionDeauthAll, resp);
        }

        public static bool checkEnteredPassword(string enteredPwd, string campus, HttpResponse resp)
        {
            // is it the District Password?
            if (isCorrectDistrictPassword(enteredPwd))
            {
                setUserAuthForCampus(authActionAuthAll, resp);
                return true;
            }
            else if (isCorrectCampusPassword(campus, enteredPwd))
            {
                setUserAuthForCampus(authActionAuthOneCampus, resp, campus);
                return true;
            }
            else
            {
                setUserAuthForCampus(authActionDeauthOneCampus, resp, campus);
                return false;
            }
        }
        //}









        //**********************************************************************//
        //** returns the password for the specified campus
        //**
        private static string getCampusPassword(string campusAbbr)
        {
            string qs =
                "select schoolpassword from aci.school " +
                "where school_abbr = \'" + campusAbbr + "\'";
            DataSet ds = new DataSet();

            ds = dbIFOracle.getDataRows(qs);

            if (ds.Tables[0] != null)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return authErrorString;
            }
        }

        private static string getCampusAdminPassword(string campusAbbr)
        {
            string schoolPassword = getCampusPassword(campusAbbr);
            string schoolAdminPassword = schoolPassword + addtlDigitForPrincipalPwd;

            return schoolAdminPassword;
        }


        private static string[] getTeacherNumber(string teacherName)
        {
            string[] retNumber = {"", "", ""};

            string qs =
                "select unique teacher_nbr " +
                "from " + birIF.dbStudentRoster +
                "where " + birIF.teacherNameFieldName +
                " = \'" + teacherName + "\'";
            DataSet ds = dbIFOracle.getDataRows(qs);

            if (ds.Tables[0] != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    retNumber[i] = ds.Tables[0].Rows[i][0].ToString();
                }
            }

            return retNumber;

        }



        //**********************************************************************//
        //** authenticates a user for the district
        //**
        private static bool isCorrectDistrictPassword(string enteredPwd)
        {
            if (enteredPwd == districtPwd || enteredPwd == principalPwd)
                return true;
            else
                return false;
        }



        //**********************************************************************//
        //** authenticates a user for a specified campus
        //**
        private static bool isCorrectCampusPassword(string campus, string enteredPwd)
        {
            string pwd = CampusSecurity.getCampusPassword(campus);

            if (enteredPwd == pwd)
                return true;
            else
                return false;
        }


        private static void setUserAuthForCampus(int operation, HttpResponse resp, string campus = "")
        {
            if (operation == authActionAuthOneCampus)
            {
                resp.Cookies[authcookiename].Value = campus;
                resp.Cookies[authcookiename].Expires = DateTime.Now.AddDays(cookieDurationDays);
            }
            else if (operation == authActionAuthAll)
            {
                resp.Cookies[authcookiename].Value = authAllCampusesValue;
                resp.Cookies[authcookiename].Expires = DateTime.Now.AddDays(cookieDurationDays);
            }
            else if (operation == authActionDeauthOneCampus)
            {
                resp.Cookies[authcookiename].Value = null;
            }
            else if (operation == authActionDeauthAll)
            {
                resp.Cookies[authcookiename].Value = null;
            }

            return;
        }


        //**********************************************************************//
        //** tell whether this user is authorized for this campus
        //**
        private static bool isAuthorizedForCampus(string campus, HttpRequest req)
        {
            bool ans = false; // (req.Cookies[authcookiename][campus] == "true") ? true : false;

            if (req.Cookies[authcookiename] != null)
                if (req.Cookies[authcookiename].Value == campus || req.Cookies[authcookiename].Value == authAllCampusesValue)
                    ans = true;

            return ans;
        }


    }


}