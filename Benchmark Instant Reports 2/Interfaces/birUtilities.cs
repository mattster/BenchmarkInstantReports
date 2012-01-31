using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Benchmark_Instant_Reports_2.References;


namespace Benchmark_Instant_Reports_2.Interfaces
{
    public class birUtilities
    {
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


        public static bool isTeacherUnknown(string studentId)
        {
            // if this is an elementary student and is not in an "Attendance" course
            if (birIF.isStudentElemNoAttCourse(studentId))
                return true;

            return false;
        }





    }
}