using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Benchmark_Instant_Reports_2.Classes;


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

        private static string savedSelectedTestIDCookieName = "selectedTestID";
        private static string savedSelectedTestIDsCokieName = "selectedTestIDs";
        private static string savedSelectedCampusCookieName = "selectedCampus";

        private static string allIndicator = "--ALL--";
        //private static string[] curricList = { "Science", "Math", "Social Studies", "Reading", "Writing", "Eng. Lang. Arts", "LOTE", "Technology", "Music", "Band / Orch." };

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
                "FROM " + birIF.dbStudentRoster + " " +
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
                "FROM " + birIF.dbStudentRoster + " " +
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
                "FROM " + birIF.dbStudentRoster + " " +
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


        public static char calcLetterGrade(string testID, int numCorrect, int numTotal, int passNum, int commendedNum)
        {
            //if (numCorrect >= commendedNum)
            //    return 'C';
            //else 
            if (numCorrect >= passNum)
                return 'P';

            return 'F';
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
                string thisString = s[i].Replace("'", "''");
                returnstring = returnstring + "\'" + thisString + "\',";
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
        //** selects all items in the specified ListBox
        //**
        public static void selectAllInLB(ListBox lb)
        {
            if (lb.Items.Count > 0)
            {
                foreach (ListItem item in lb.Items)
                {
                    item.Selected = true;
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


        //**********************************************************************//
        //** returns a list of school types applicable for this school;
        //** a list in the form of 'E','S'; or simply 'E' for just one
        //**
        public static string getSchoolTypeList(string schoolAbbr)
        {
            if (schoolAbbr == "ALL Elementary")
            {
                return "\'E\'";
            }
            else if (schoolAbbr == "ALL Secondary")
            {
                return "\'S\'";
            }

            else
            {

                string qs = "select schoolid from " + birIF.dbSchool + " " +
                    "where school_abbr = \'" + schoolAbbr + "\'";

                System.Data.DataSet ds = dbIFOracle.getDataRows(qs);

                int schoolId = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                if (schoolId >= 1 && schoolId <= 99 && schoolId != 6)
                    return "\'S\'";
                else if (schoolId >= 100)
                    return "\'E\'";
                else if (schoolId == 6)
                    return "\'E\',\'S\'";
                else
                    return "\'E\',\'S\'";
            }
        }


        public static void updateAuthDisplayInfo(HttpRequest req)
        {

        }


        public static DataTable getFilteredTable(DataTable bigTable, string filterCriteria)
        {
            DataRow[] rMatches = bigTable.Select(filterCriteria);
            DataTable dtMatches = bigTable.Clone();
            foreach (DataRow row in rMatches)
            {
                DataRow newrow = dtMatches.NewRow();
                newrow.ItemArray = row.ItemArray;
                dtMatches.Rows.Add(newrow);
            }

            return dtMatches;
        }


        public static int getIndexOfDDItem(string itemName, DropDownList ddl)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
                if (ddl.Items[i].Text.Trim() == itemName)
                    return i;

            return -1;
        }

        private static int[] getIndicesOfLBItems(string[] itemNames, ListBox lb)
        {
            List<int> indicesList = new List<int>();
            for (int i = 0; i < lb.Items.Count; i++)
                if (isStringInStringArray(lb.Items[i].Text, itemNames))
                    indicesList.Add(i);

            if (indicesList.Count > 0)
                return indicesList.ToArray();
            else
                return null;
        }

        public static void selectItemsInLB(ListBox lb, string[] items)
        {
            int[] indices = getIndicesOfLBItems(items, lb);
            if (indices != null)
                selectItemsInLB(lb, indices);

            return;
        }

        private static void selectItemsInLB(ListBox lb, int[] indices)
        {
            for (int i = 0; i < indices.Length; i++)
                lb.Items[indices[i]].Selected = true;

            return;
        }



        public static bool isStringInStringArray(string str, string[] strArray)
        {
            for (int i = 0; i < strArray.Length; i++)
                if (strArray[i] == str)
                    return true;

            return false;
        }


        public static string savedSelectedTestID(HttpRequest req)
        {
            if (req.Cookies[savedSelectedTestIDCookieName] != null)
                return req.Cookies[savedSelectedTestIDCookieName].Value;

            return null;
        }
        public static void savedSelectedTestID(HttpResponse resp, string testID)
        {
            resp.Cookies[savedSelectedTestIDCookieName].Value = testID;
            resp.Cookies[savedSelectedTestIDCookieName].Expires = DateTime.Now.AddDays(CampusSecurity.cookieDurationDays);
            resp.Cookies[savedSelectedTestIDCookieName].Path = "/";

            return;
        }



        public static string savedSelectedCampus(HttpRequest req)
        {
            if (req.Cookies[savedSelectedCampusCookieName] != null)
                return req.Cookies[savedSelectedCampusCookieName].Value;

            return null;
        }
        public static void savedSelectedCampus(HttpResponse resp, string campus)
        {
            resp.Cookies[savedSelectedCampusCookieName].Value = campus;
            resp.Cookies[savedSelectedCampusCookieName].Expires = DateTime.Now.AddDays(CampusSecurity.cookieDurationDays);
            resp.Cookies[savedSelectedCampusCookieName].Path = "/";

            return;
        }


        public static string[] savedSelectedTestIDs(HttpRequest req)
        {
            if (req.Cookies[savedSelectedTestIDsCokieName] != null)
                return req.Cookies[savedSelectedTestIDsCokieName].Value.Split(',');

            return null;
        }
        public static void savedSelectedTestIDs(HttpResponse resp, string[] tests)
        {
            resp.Cookies[savedSelectedTestIDsCokieName].Value = string.Join(",", tests);
            resp.Cookies[savedSelectedTestIDsCokieName].Expires = DateTime.Now.AddDays(CampusSecurity.cookieDurationDays);
            resp.Cookies[savedSelectedTestIDsCokieName].Path = "/";

            return;
        }

        public static bool isTeacherUnknown(string studentId)
        {
            // if this is an elementary student and is not in an "Attendance" course
            if (birIF.isStudentElemNoAttCourse(studentId))
                return true;

            return false;
        }

        private static string[] getCurricList(string campus)
        {
            List<string> curriclist = new List<string>();
            string schtype = birIF.getSchoolType(campus);

            curriclist.Add(allIndicator);

            if (schtype == "A")                 // both Elem & Sec
            {
                foreach (Curriculum curric in AllTestMetadata.AllCurriculum)
                {
                    curriclist.Add(curric.DispAbbr);
                }
            }
            else if (schtype == "E")            // Elementary
            {
                foreach (Curriculum curric in AllTestMetadata.AllCurriculum)
                {
                    if (curric.ElemSec == "E" || curric.ElemSec == "B")
                        curriclist.Add(curric.DispAbbr);
                }
            }
            else                                // Secondary
            {
                foreach (Curriculum curric in AllTestMetadata.AllCurriculum)
                {
                    if (curric.ElemSec == "S" || curric.ElemSec == "B")
                        curriclist.Add(curric.DispAbbr);
                }
            }

            return curriclist.ToArray();
        }

        private static void loadCurricListInDD(DropDownList ddl, string campus)
        {
            ddl.DataSource = getCurricList(campus);
            //ddl.SelectedIndex = 0;
            ddl.DataBind();

            return;
        }

        public static void setupTestFilterPopup(DropDownList ddTFCur, string campus)
        {
            loadCurricListInDD(ddTFCur, campus);

            return;
        }


        internal static bool filterTestsByCurric(string campus, DropDownList ddl, string curric)
        {

            string[] alltests = birIF.getTestListForSchool(campus);
            List<string> resultList = new List<string>();
            string pattern = getRegExPatternForCurric(curric);

            foreach (string curTest in alltests)
            {
                if (Regex.IsMatch(curTest, pattern) || curric == allIndicator)
                    resultList.Add(curTest);
            }

            
            ddl.DataSource = resultList;
            ddl.DataBind();

            return (curric == allIndicator) ?  false :  true;
        }

        private static string getRegExPatternForCurric(string curric)
        {
            foreach (Curriculum thisCurric in AllTestMetadata.AllCurriculum)
            {
                if (thisCurric.DispAbbr == curric)
                    return thisCurric.RegEx;
            }

            return ".*";
        }


        public static DataView getAuthorizedCampusList(string username)
        {
            DataSet ds = new DataSet();

            if (username == birIF.usernameAllCampuses)
            {
                ds = dbIFOracle.getDataRows(birIF.getCampusListQuery);
             }
            else
            {
                string qs = birIF.getCampusInfoForCampus.Replace("@schoolAbbr", username);
                ds = dbIFOracle.getDataRows(qs);
            }

            DataView dv = new DataView(ds.Tables[0]);
            return dv;
        }


        public static void setFilterButtonImage(Image imgFilterTests, bool filtersapplied)
        {
            if (filtersapplied)
            {
                imgFilterTests.ImageUrl = "~/content/images/f-circ-red-20x20.png";
            }
            else
            {
                imgFilterTests.ImageUrl = "~/content/images/f-circ-20x20.png";
            }

            return;
        }
    }
}