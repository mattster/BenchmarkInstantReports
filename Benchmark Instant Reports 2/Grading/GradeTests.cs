using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Exceptions;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class GradeTests
    {
        public static List<GradedItemDetail> GradeTestItemsInDetail(string testID, string studentAnswerString, string curCampus, 
                                                                     int ansKeyIncAmt, string teacher, string period)
        {
            int itemObjective = new int();
            int curItemNum = new int();

            //convert answer string to an array
            string[] studentAnswerStringArray = studentAnswerString.Split(',');

            // do special processing for special template types
            if (GridHandler.isGriddable(testID))
                GridHandler.ProcessAnswerStringWithGrids(studentAnswerStringArray, testID, curCampus);

            if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(testID))
                MultiAnswerTemplateHandler.ProcessAnswerStringWithMultiAnswers(studentAnswerStringArray, testID, curCampus);

            List<AnswerKeyItem> theAnswerKey = AnswerKey.getTestAnswerKey(testID, curCampus, ansKeyIncAmt, teacher, period);

            List<GradedItemDetail> finalData = new List<GradedItemDetail>();

            //grade each item on the test
            int numTotal = theAnswerKey.Count;
            for (int i = 0; i < numTotal; i++)
            {
                string correctAnswer = theAnswerKey[i].Answer;
                itemObjective = theAnswerKey[i].Category;
                string itemTEKS = theAnswerKey[i].TEKS;
                curItemNum = theAnswerKey[i].ItemNum;

                GradedItemDetail newItem = new GradedItemDetail();
                newItem.ItemNum = curItemNum;
                newItem.CorrectAnswer = correctAnswer;
                newItem.Category = itemObjective;
                newItem.TEKS = itemTEKS;

                if (studentAnswerStringArray.Length >= curItemNum)
                {                                                       // a student answer exists
                    string studentAnswer = studentAnswerStringArray[curItemNum - 1];
                    studentAnswer = (studentAnswer.Trim() == "") ? "-" : studentAnswer;
                    newItem.StudentAnswer = studentAnswer;
                    
                    if (studentAnswer == correctAnswer)
                    {                                                   // correct answer
                        newItem.Correct = true;
                    }
                    else if (studentAnswer == ExceptionHandler.getAlternateAnswer(testID, curItemNum, curCampus))
                    {
                        newItem.Correct = true;
                    }
                    else
                    {                                                   // incorrect answer
                        newItem.Correct = false;
                    }
                }
                else
                {                                                       // no student answer
                    newItem.Correct = false;
                }
                finalData.Add(newItem);
            }

            return finalData;
        }


        public static GradedTestData GradeTest(string testID, string studentAnswerString, string curCampus, int ansKeyIncAmt, 
                                                         string teacher, string period)
        {
            int numCorrect, numTotal = new int();
            decimal numPoints, numTotalPoints = new decimal();
            int curItemNum = new int();
            decimal pctCorrect = new decimal();
            char letterGrade = new char();

            //get pass and commended numbers for this test
            int passNum = birIF.getTestPassingNum(testID);
            int commendedNum = birIF.getTestCommendedNum(testID);

            //convert answer string to an array
            string[] studentAnswerStringArray = studentAnswerString.Split(',');

            // do special processing for special template types
            if (GridHandler.isGriddable(testID))
                GridHandler.ProcessAnswerStringWithGrids(studentAnswerStringArray, testID, curCampus);

            if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(testID))
                MultiAnswerTemplateHandler.ProcessAnswerStringWithMultiAnswers(studentAnswerStringArray, testID, curCampus);

            List<AnswerKeyItem> theAnswerKey = AnswerKey.getTestAnswerKey(testID, curCampus, ansKeyIncAmt, teacher, period);

            //grade each item on the test
            numCorrect = 0;
            numPoints = 0;
            numTotalPoints = 0;
            numTotal = theAnswerKey.Count;

            //for (int i = 0; i < numTotal; i++)
            foreach (AnswerKeyItem itemAnswerKey in theAnswerKey)
            {
                curItemNum = itemAnswerKey.ItemNum;

                if (studentAnswerStringArray.Length >= curItemNum)       // a student answer exists
                {
                    if (studentAnswerStringArray[curItemNum - 1] == itemAnswerKey.Answer)
                    {
                        // student's answer is correct
                        numCorrect++;
                        numPoints += itemAnswerKey.Weight;
                    }
                    else if (studentAnswerStringArray[curItemNum - 1] == ExceptionHandler.getAlternateAnswer(testID, curItemNum, curCampus))
                    {
                        // student's answer is correct as the alternate answer
                        numCorrect++;
                        numPoints += itemAnswerKey.Weight;
                    }
                }

                numTotalPoints += itemAnswerKey.Weight;
            }

            //calculate stuff
            pctCorrect = numPoints / numTotalPoints;
            letterGrade = CalcLetterGrade(numPoints, passNum);

            //return the results
            GradedTestData newItem = new GradedTestData();
            newItem.LetterGrade = letterGrade.ToString();
            newItem.NumCorrect = numCorrect;
            newItem.NumTotal = numTotal;
            newItem.NumPoints = numPoints;
            newItem.NumTotalPoints = numTotalPoints;
            newItem.PctCorrect = pctCorrect;
            newItem.PassNum = passNum;
            newItem.CommendedNum = commendedNum;

            //return table;
            return newItem;
        }


        public static void addStudentAnswerData(StGradeReportData gradeddata, string testid, string curCampus)
        {
            int ansKeyVersionIncrement = new int();

            // go through each student and create the answer strings we need
            for (int i = 0; i < gradeddata.Count; i++)
            {
                ScanItem curScanDataItem = birIF.getLatestScanDataRowq(gradeddata.Idx(i).StudentID, gradeddata.Idx(i).TestID);
                if (curScanDataItem != null)
                {
                    ansKeyVersionIncrement = ExceptionHandler.campusAnswerKeyVersionIncrement(gradeddata.Idx(i).TestID, 
                        gradeddata.Idx(i).Campus, gradeddata.Idx(i).Teacher, gradeddata.Idx(i).Period);
                    List<GradedItemDetail> gradeditemsdetails = GradeTestItemsInDetail(testid, curScanDataItem.Answers,
                        curCampus, ansKeyVersionIncrement, gradeddata.Idx(i).Teacher, gradeddata.Idx(i).Period);

                    // go through each answer and create the strings we need
                    string curGradedAnsString = "";
                    foreach (GradedItemDetail curgradeditem in gradeditemsdetails)
                    {
                        if (curgradeditem.Correct)
                            curGradedAnsString = curGradedAnsString + "*,";
                        else
                            // ****** put either the student's answer here, or the actual correct answer ******
                            curGradedAnsString = curGradedAnsString + curgradeditem.StudentAnswer + ",";
                    }
                    curGradedAnsString = curGradedAnsString.Substring(0, curGradedAnsString.Length - 1);

                    // make the fancy shmancy string for the report
                    string formattedAnsString = createStudentGradedAnsString(curGradedAnsString, gradeditemsdetails);

                    //update gradeddata 
                    StGradeReportItem curReportItem = gradeddata.Idx(i);
                    curReportItem.GradedAnswers = curGradedAnsString;
                    curReportItem.GradedAnswersFormatted = formattedAnsString;
                    gradeddata.UpdateItemAtIndexWith(i, curReportItem);
                }
            }

            return;
        }






        private static string createStudentGradedAnsString(string rawAnsString, List<GradedItemDetail> gradeditems)
        {
            string[] resultsString = new string[Constants.MaxFormattedAnsGroups];
            string[] titleString = new string[Constants.MaxFormattedAnsGroups];
            string formattedString = "";
            int rowIndex = 0;
            
            string[] ansStringArray = rawAnsString.Split(',');

            for (int i = 0; i < ansStringArray.Length; i++)
            {
                // write this one
                if ((i + 1).ToString().Length <= 2)
                {
                    titleString[rowIndex] = titleString[rowIndex] + string.Format("{0,2} ", gradeditems[i].ItemNum); 
                    resultsString[rowIndex] = resultsString[rowIndex] + string.Format("{0,2} ", ansStringArray[i]);
                }
                else
                {
                    titleString[rowIndex] = titleString[rowIndex] + string.Format("{0,3} ", gradeditems[i].ItemNum);
                    resultsString[rowIndex] = resultsString[rowIndex] + string.Format("{0,3} ", ansStringArray[i]);
                }


                // check if the next one will fit on this row
                if ((i + 1) < ansStringArray.Length)
                {
                    if ((Constants.NumColumnsInFormattedLine - 
                        (resultsString[rowIndex].Length + gradeditems[i + 1].ItemNum.ToString().Length)) < 2)
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




        private static char CalcLetterGrade(decimal pointsfor, int passnum)
        {
            if (pointsfor >= (decimal)passnum)
                return 'P';

                // if the pointsfor number will round up to the (int)passnum to the nearest tenth,
                //   i.e. if the difference is less than or equal to 0.05,
                //   then call this a Pass since it will display the points as such
            else if ((decimal)passnum - pointsfor <= (decimal)0.5)
                return 'P';

            return 'F';
        }
    }
}