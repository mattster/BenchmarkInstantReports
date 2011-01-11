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
    public class CampusSecurity
    {
        private static int authActionAuthOneCampus = 1;                      // authorize a user for 1 campus
        private static int authActionDeauthOneCampus = -1;                   // deauthorize a user for 1 campus
        private static int authActionAuthAll = 99;                           // authorize a user for all campuses
        private static int authActionDeauthAll = 0;                          // de-authorize a user for all campuses
        private static string districtPwd = "28774";                         // district password
        private static string authErrorString = "xxxxxxxxxxxxxxxxxxxxERRORxxxxxxxxxxxxxxxxxxxx";


        //*******************
        //* class to manage the authentication list for
        //* a user's session
        //********
        public class campusAuthList
        {
            private DataSet dsAuthList;
            
            // default constructor
            public campusAuthList()
            {
                dsAuthList = new DataSet();
                setUserAuthForCampus(authActionDeauthAll, dsAuthList);
            }

            public bool isAuthorized(string campus)
            {
                return isAuthorizedForCampus(campus, dsAuthList);
            }

            public void setAuthForCampus(string campus)
            {
                setUserAuthForCampus(authActionAuthOneCampus, dsAuthList, campus);
                return;
            }

            public void revokeAuthForCampus(string campus)
            {
                setUserAuthForCampus(authActionDeauthOneCampus, dsAuthList, campus);
                return;
            }

            public void revokeAuthAll()
            {
                setUserAuthForCampus(authActionDeauthAll, dsAuthList);
                return;
            }

            public void setAuthAll()
            {
                setUserAuthForCampus(authActionAuthAll, dsAuthList);
                return;
            }

            public bool checkEnteredPassword(string enteredPwd, string campus)
            {
                // is it the District Password?
                if (isCorrectDistrictPassword(enteredPwd))
                {
                    setUserAuthForCampus(authActionAuthAll, dsAuthList);
                    return true;
                }
                else if (isCorrectCampusPassword(campus, enteredPwd))
                {
                    setUserAuthForCampus(authActionAuthOneCampus, dsAuthList, campus);
                    return true;
                }
                else
                {
                    setUserAuthForCampus(authActionDeauthOneCampus, dsAuthList, campus);
                    return false;
                }
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
        public static bool isCorrectDistrictPassword(string enteredPwd)
        {
            if (enteredPwd == districtPwd)
                return true;
            else
                return false;
        }



        //**********************************************************************//
        //** authenticates a user for a specified campus
        //**
        public static bool isCorrectCampusPassword(string campus, string enteredPwd)
        {
            string pwd = CampusSecurity.getCampusPassword(campus);

            if (enteredPwd == pwd)
                return true;
            else
                return false;
        }


        //**********************************************************************//
        //** update user's authentication status for this session
        //**
        private static void setUserAuthForCampus(int operation, DataSet dsAuth, string campus = "")
        {
            int i = new int();
            string colname = "";

            // initialize the authorization dataset if needed
            if (dsAuth.Tables.Count < 1)
            {
                string qs = "select school_abbr from aci.school order by school_abbr";
                DataSet dstemp = new DataSet();
                dstemp = dbIFOracle.getDataRows(qs);
                dsAuth.Tables.Add();

                // add the columns
                for (i = 0; i < dstemp.Tables[0].Rows.Count; i++)
                {
                    colname = dstemp.Tables[0].Rows[i][0].ToString();
                    dsAuth.Tables[0].Columns.Add(colname, System.Type.GetType("System.Boolean"));
                }

                // add a row, set everything to false
                DataRow newrow = dsAuth.Tables[0].NewRow();
                foreach (DataColumn col1 in dsAuth.Tables[0].Columns)
                {
                    newrow[col1.ColumnName] = false;
                }
                dsAuth.Tables[0].Rows.Add(newrow);
            }
            
            // do what they want us to do
            if (operation == authActionAuthOneCampus)
            {
                dsAuth.Tables[0].Rows[0][campus] = true;
            }

            else if (operation == authActionDeauthOneCampus)
            {
                dsAuth.Tables[0].Rows[0][campus] = false;
            }

            else if (operation == authActionAuthAll)
            {
                foreach (DataColumn col2 in dsAuth.Tables[0].Columns)
                    dsAuth.Tables[0].Rows[0][col2.ColumnName.ToString()] = true;
            }

            else                                                // deauthorize all by default
            {
                foreach (DataColumn col2 in dsAuth.Tables[0].Columns)
                    dsAuth.Tables[0].Rows[0][col2.ColumnName.ToString()] = false;

            }

            return;

        }


        //**********************************************************************//
        //** tell whether this user is authorized for this campus
        //**
        private static bool isAuthorizedForCampus(string campus, DataSet dsAuth)
        {
            bool ans = (bool)dsAuth.Tables[0].Rows[0][campus];
            return ans;
        }


    }


}