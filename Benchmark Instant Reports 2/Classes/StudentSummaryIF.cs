using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Benchmark_Instant_Reports_2.Exceptions;


//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
// an interface class for the Student Summary report
//
// matt hollingsworth
// oct 2010
//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *



namespace Benchmark_Instant_Reports_2
{
    public class StudentSummaryIF
    {

        public static string studentStatsResultsDatatableName = "ACI.TEMP_RESULTS_STUDENTSTATS";
        private static int maxFormattedAnsGroups = 7;
        private static int numColumnsInFormattedLine = 75;


        //**********************************************************************//
        //** add two fields to an existing data table that has student results
        //** in it; the two fields are:
        //** graded_answers, graded_answers_formatted
        //**
        public static void addStudentAnswerData(DataTable theTable, string theTest, string curCampus)
        {
            string curId = "";
            string curTeacher = "";
            string curPeriod = "";
            string lblGradedAnswers = "GRADED_ANSWERS";
            string lblGradedAnswersFmtd = "GRADED_ANSWERS_FORMATTED";
            int ansKeyVersionIncrement = new int();
            string[] courseList = birIF.getCourseIDsForTest(theTest);

            theTable.Columns.Add(lblGradedAnswers, System.Type.GetType("System.String"));
            theTable.Columns.Add(lblGradedAnswersFmtd, System.Type.GetType("System.String"));
            theTable.Columns[lblGradedAnswersFmtd].MaxLength = 1500;

            // go through each student and create the answer strings we need
            for (int j = 0; j < theTable.Rows.Count; j++)
            {
                curId = theTable.Rows[j]["STUDENT_ID"].ToString();
                curTeacher = theTable.Rows[j]["TEACHER"].ToString();
                curPeriod = theTable.Rows[j]["PERIOD"].ToString();

                DataRow curScanDataRow = birIF.getLatestScanDataRow(curId, theTest);
                if (curScanDataRow != null)
                {
                    //string[] thisTeacherPeriod = courseList.Length == 0 ? new string[] { "UNKNOWN", "01" } : birIF.getTeacherPeriodForStudentInCourses(curId, courseList);

                    ansKeyVersionIncrement = ExceptionHandler.campusAnswerKeyVersionIncrement(theTest, curCampus,
                    curTeacher, curPeriod);
                    DataTable gradedTable = birIF.gradeScannedTestDetail(theTest, curScanDataRow["ANSWERS"].ToString(), curCampus,
                        ansKeyVersionIncrement, curTeacher, curPeriod);

                    // go through each answer and create the strings we need
                    string curGradedAnsString = "";
                    for (int k = 0; k < gradedTable.Rows.Count; k++)
                    {
                        if ((bool)gradedTable.Rows[k]["CORRECT"])
                            curGradedAnsString = curGradedAnsString + "*,";
                        else
                            // ****** put either the student's answer here, or the actual correct answer ******
                            curGradedAnsString = curGradedAnsString + gradedTable.Rows[k]["STUDENT_ANS"].ToString() + ",";
                    }
                    curGradedAnsString = curGradedAnsString.Substring(0, curGradedAnsString.Length - 1);

                    // make the fancy shmancy string for the report
                    string formattedAnsString = createStudentGradedAnsString(curGradedAnsString, gradedTable);

                    //add this to the data table
                    theTable.Rows[j][lblGradedAnswers] = curGradedAnsString;
                    theTable.Rows[j][lblGradedAnswersFmtd] = formattedAnsString;
                }
            }

            return;
        }


        //**********************************************************************//
        //** create a printable string of graded anwers for a student for use
        //** on the Student Summary Report
        //**
        public static string createStudentGradedAnsString(string rawAnsString, DataTable answerTable)
        {
            string[] resultsString = new string[maxFormattedAnsGroups];
            string[] titleString = new string[maxFormattedAnsGroups];
            string formattedString = "";
            int rowIndex = 0;
            string lblItemNum = "ITEM_NUM";

            string[] ansStringArray = rawAnsString.Split(',');

            for (int i = 0; i < ansStringArray.Length; i++)
            {
                // write this one
                if ((i + 1).ToString().Length <= 2)
                {
                    titleString[rowIndex] = titleString[rowIndex] + string.Format("{0,2} ", answerTable.Rows[i][lblItemNum]);
                    resultsString[rowIndex] = resultsString[rowIndex] + string.Format("{0,2} ", ansStringArray[i]);
                }
                else
                {
                    titleString[rowIndex] = titleString[rowIndex] + string.Format("{0,3} ", answerTable.Rows[i][lblItemNum]);
                    resultsString[rowIndex] = resultsString[rowIndex] + string.Format("{0,3} ", ansStringArray[i]);
                }


                // check if the next one will fit on this row
                if ((i + 1) < ansStringArray.Length)
                {
                    if ((numColumnsInFormattedLine - (resultsString[rowIndex].Length + answerTable.Rows[i + 1][lblItemNum].ToString().Length)) < 2)
                    {
                        // next item will not fit; delete extra space at end and go to next row
                        titleString[rowIndex] = titleString[rowIndex].Substring(0, titleString[rowIndex].Length - 1);
                        resultsString[rowIndex] = resultsString[rowIndex].Substring(0, resultsString[rowIndex].Length - 1);
                        rowIndex++;
                    }
                }
            }

            // put the rows together, insert newlines
            for (int j = 0; j <= rowIndex; j++)
            {
                formattedString = formattedString + String.Format("{0}\n{1}\n", titleString[j], resultsString[j]);
            }

            // remove the last newline - we don't need it
            formattedString = formattedString.Substring(0, formattedString.Length - 1);

            return formattedString;
        }



    }
}