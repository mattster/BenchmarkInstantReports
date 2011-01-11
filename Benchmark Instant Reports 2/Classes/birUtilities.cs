using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
// a class with a collection of useful stuff for the Benchmark Instant Reports
//
// matt hollingsworth
// oct 2010
//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *


namespace Benchmark_Instant_Reports_2
{
    public class birUtilities
    {

        public static CampusSecurity.campusAuthList sessionAuthList = new CampusSecurity.campusAuthList();

        //**********************************************************************//
        //** returns an array of unique values in a column of the specified
        //** table for a string
        //**
        public static string[] getUniqueTableColumnStringValues(DataTable theTable, string columnName)
        {
            object[] array1 = new object[theTable.Rows.Count];
            int columnNum = theTable.Columns.IndexOf(columnName);

            for (int i = 0; i < theTable.Rows.Count; i++)
            {
                array1[i] = theTable.Rows[i].ItemArray[columnNum];
            }
            IEnumerable<object> returnArrayTemp = array1.Distinct();
            string[] returnArray = new string[returnArrayTemp.Count()];

            int j = 0;
            foreach (object item in returnArrayTemp)
            {
                if (item != null)
                    returnArray[j] = item.ToString();
                j++;
            }

            return returnArray;
        }
        
 
        //**********************************************************************//
        //** determines whether this is a valid student id -- that is, does
        //** this student id exist in the student roster
        //**
        public static bool isValidStudentId(string studentId)
        {
            string qs =
                "SELECT LOCAL_STUDENT_ID " +
                "FROM SIS_ODS.RISD_STUDENT_ROSTER_VW " +
                "WHERE LOCAL_STUDENT_ID = \'" + studentId + "\'";

            DataSet ds = dbIFOracle.getDataRows(qs);

            if (ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }


        //**********************************************************************//
        //** lookup a student's name based on their ID
        //**
        public static string lookupStudentName(string studentId)
        {
            string qs = "SELECT STUDENT_NAME, LOCAL_STUDENT_ID " +
                "FROM SIS_ODS.RISD_STUDENT_ROSTER_VW " +
                "WHERE LOCAL_STUDENT_ID = \'" + studentId + "\' " +
                "AND ROWNUM = 1";

            DataSet ds = dbIFOracle.getDataRows(qs);

            return ds.Tables[0].Rows[0][0].ToString();
        }


        //**********************************************************************//
        //** lookup a student's campus based on their ID
        //** note: this assumes that the student's campus is the same
        //** for every record in the student roster
        //**
        public static string lookupStudentCampus(string studentId)
        {
            string qs =
                "SELECT SCHOOL_ABBR, LOCAL_STUDENT_ID " +
                "FROM SIS_ODS.RISD_STUDENT_ROSTER_VW " +
                "WHERE LOCAL_STUDENT_ID = \'" + studentId + "\' " +
                "AND ROWNUM = 1";

            DataSet ds = dbIFOracle.getDataRows(qs);

            return ds.Tables[0].Rows[0][0].ToString();
        }


        //**********************************************************************//
        //** calculate a letter grade for a given test and a given
        //** number correct
        //**
        public static char calcLetterGrade(string testID, int numCorrect, int numTotalGiven, bool lookupTestInfo)
        {
            int numTotalForCalc = 100;
            
            if (lookupTestInfo)
            {
                // get numTotal from test definition
                DataSet ds = new DataSet();
                string q = birIF.numTestQuestionsQuery.Replace("@testId", testID);
                ds = dbIFOracle.getDataRows(q);
                numTotalForCalc = (int)ds.Tables[0].Rows[0][0];
            }
            else
            {
                // get numTotal from values passed to the method
                numTotalForCalc = numTotalGiven;
            }                
                
            decimal calc = 100 * (decimal)numCorrect / (decimal)numTotalForCalc;
            int pct = (int)Math.Round(calc, 0);

            if (pct >= 90)
                return 'A';
            else if ((pct >= 80) && (pct < 90))
                return 'B';
            else if ((pct >= 70) && (pct < 80))
                return 'C';
            else if (pct < 70)
                return 'F';
            else
                return 'X';
        }

 
        //**********************************************************************//
        //** returns a simple string with single quotes around and commas
        //** between the values in a string array
        //**
        public static string convertStringArrayForQuery(string[] s)
        {
            string returnstring = "";

            for (int i = 0; i < s.Length; i++)
            {
                returnstring = returnstring + "\'" + s[i] + "\',";
            }
            returnstring = returnstring.Substring(0, returnstring.Length - 1);

            return returnstring;
        }


        //**********************************************************************//
        //** returns a string array of the values selected in a listbox
        //**
        public static string[] getLBSelectionsAsArray(System.Web.UI.WebControls.ListBox lb)
        {
            string[] returnstring = new string[lb.GetSelectedIndices().Count()];
            int[] indices = lb.GetSelectedIndices();

            for (int i = 0; i < indices.Length; i++)
            {
                returnstring[i] = lb.Items[indices[i]].ToString();
            }
           
            return returnstring;
        }


        //**********************************************************************//
        //** toggles whether to display a blank first row of the specified
        //** dropdown control - used for nicer UI experience
        //** initial: if TRUE then set first row to blank
        //** initial: if FALSE then remove first blank row if there is one
        //**
        public static void toggleDDLInitView(DropDownList ddl, bool initial)
        {
            if (ddl.Items.Count > 0)                    // make sure it has something in it
            {
                //
                if (initial)                            // put in blank 1st row, select it
                {
                    if (ddl.Items[0].Value != "-1")     // there is not a blank first row
                        ddl.Items.Insert(0, new ListItem(" ", "-1"));
                    ddl.SelectedIndex = 0;
                }
                else                                    // remove blank 1st row if necessary
                {
                    if (ddl.Items[0].Value == "-1")     // there is a blank 1st row
                        ddl.Items.RemoveAt(0);
                }
            }

            return;
        }


        //**********************************************************************//
        //** if this is a separator value from the campus dropdown,
        //** return true, else return false
        //**
        public static bool isDDSeparatorValue(string selection)
        {
            if (selection == birIF.dropDownSeparatorString)
                return true;
            else
                return false;
        }
    
    
    
    
    }
}