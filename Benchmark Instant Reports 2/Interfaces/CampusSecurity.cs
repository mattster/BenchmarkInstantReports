using System.Data;
using System.Web;
using Benchmark_Instant_Reports_2.References;



namespace Benchmark_Instant_Reports_2.Interfaces
{
    public static class CampusSecurity
    {
        private static string districtPwd = "28774";                         // district password
        private static string principalPwd = "40996";                       // principal password
        private static string authErrorString = "xxxxxxxxxxxxxxxxxxxxERRORxxxxxxxxxxxxxxxxxxxx";
        public static string authcookiename = "campusauthlist";            // name of the cookie
        public static string authAllCampusesValue = "All";
        public static int cookieDurationDays = 5;

        public static bool isAuthorizedAsAdmin(string username)
        {
            if (username == Constants.UsernameAllCampuses)
                return true;

            return false;
        }

        public static bool checkEnteredPassword(string enteredPwd, string campus, HttpResponse resp)
        {
            // is it the District Password?
            if (isCorrectDistrictPassword(enteredPwd))
            {
                return true;
            }
            else if (isCorrectCampusPassword(campus, enteredPwd))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


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

    }


}