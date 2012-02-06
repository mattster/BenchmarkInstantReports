using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Exceptions;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class GradeTests
    {
        //public static List<GradedItemDetail> GradeTestItemsInDetail(string testID, string studentAnswerString, string curCampus, 
        //                                                             int ansKeyIncAmt, string teacher, string period)
        //{
        //    int itemObjective = new int();
        //    int curItemNum = new int();

        //    //convert answer string to an array
        //    string[] studentAnswerStringArray = studentAnswerString.Split(',');

        //    // do special processing for special template types
        //    if (GridHandler.isGriddable(testID))
        //        GridHandler.ProcessAnswerStringWithGrids(studentAnswerStringArray, testID, curCampus);

        //    if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(testID))
        //        MultiAnswerTemplateHandler.ProcessAnswerStringWithMultiAnswers(studentAnswerStringArray, testID, curCampus);

        //    List<AnswerKeyItem> theAnswerKey = AnswerKeyHelper.getTestAnswerKey(testID, curCampus, ansKeyIncAmt, teacher, period);

        //    List<GradedItemDetail> finalData = new List<GradedItemDetail>();

        //    //grade each item on the test
        //    int numTotal = theAnswerKey.Count;
        //    for (int i = 0; i < numTotal; i++)
        //    {
        //        string correctAnswer = theAnswerKey[i].Answer;
        //        itemObjective = theAnswerKey[i].Category;
        //        string itemTEKS = theAnswerKey[i].TEKS;
        //        curItemNum = theAnswerKey[i].ItemNum;

        //        GradedItemDetail newItem = new GradedItemDetail();
        //        newItem.ItemNum = curItemNum;
        //        newItem.CorrectAnswer = correctAnswer;
        //        newItem.Category = itemObjective;
        //        newItem.TEKS = itemTEKS;

        //        if (studentAnswerStringArray.Length >= curItemNum)
        //        {                                                       // a student answer exists
        //            string studentAnswer = studentAnswerStringArray[curItemNum - 1];
        //            studentAnswer = (studentAnswer.Trim() == "") ? "-" : studentAnswer;
        //            newItem.StudentAnswer = studentAnswer;
                    
        //            if (studentAnswer == correctAnswer)
        //            {                                                   // correct answer
        //                newItem.Correct = true;
        //            }
        //            else if (studentAnswer == ExceptionHandler.getAlternateAnswer(testID, curItemNum, curCampus))
        //            {
        //                newItem.Correct = true;
        //            }
        //            else
        //            {                                                   // incorrect answer
        //                newItem.Correct = false;
        //            }
        //        }
        //        else
        //        {                                                       // no student answer
        //            newItem.Correct = false;
        //        }
        //        finalData.Add(newItem);
        //    }

        //    return finalData;
        //}


        /// <summary>
        /// scores a student's answers for a specified test against the specified answer key
        /// </summary>
        /// <param name="test">Test object of the test to grade</param>
        /// <param name="studentAnswerString">student's answers in a raw string</param>
        /// <param name="ansKey">AnswerKeyItemData answer key</param>
        /// <returns>GradedTestData collection of graded items</returns>
        public static GradedTestData GradeStudentAnswers(Test test, string studentAnswerString, AnswerKeyItemData ansKey)
        {
            int numCorrect, numTotal = new int();
            double numPoints, numTotalPoints = new double();
            int curItemNum = new int();
            double pctCorrect = new double();
            char letterGrade = new char();

            // convert answer string to an array
            string[] studentAnswerStringArray = studentAnswerString.Split(',');

            // do special processing for special template types
            if (GridHandler.isGriddable(test.TestID))
                GridHandler.ProcessAnswerStringWithGrids(studentAnswerStringArray, test.TestID);

            if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(test.TestID))
                MultiAnswerTemplateHandler.ProcessAnswerStringWithMultiAnswers(studentAnswerStringArray, test.TestID);

            // grade each item on the test
            numCorrect = 0;
            numPoints = 0;
            numTotalPoints = 0;
            numTotal = ansKey.Count;
            string curGradedAnsString = "";

            foreach (AnswerKeyItem itemAnswerKey in ansKey.GetItems())
            {
                curItemNum = itemAnswerKey.ItemNum;
                string studentAnswer = studentAnswerStringArray[curItemNum - 1];

                if (studentAnswerStringArray.Length >= curItemNum)       // a student answer exists
                {
                    if (studentAnswer == itemAnswerKey.Answer)
                    {
                        // student's answer is correct
                        numCorrect++;
                        numPoints += itemAnswerKey.Weight;
                        curGradedAnsString = curGradedAnsString + "*,";
                    }
                    else if (studentAnswer ==
                        ExceptionHandler.getAlternateAnswer(test.TestID, curItemNum, itemAnswerKey.Campus))
                    {
                        // student's answer is correct as the alternate answer
                        numCorrect++;
                        numPoints += itemAnswerKey.Weight;
                        curGradedAnsString = curGradedAnsString + "*,";
                    }
                    else
                    {
                        // student's answer is incorrect
                        curGradedAnsString = curGradedAnsString + studentAnswer + ",";
                    }
                }
                curGradedAnsString = curGradedAnsString.Substring(0, curGradedAnsString.Length - 1);

                numTotalPoints += itemAnswerKey.Weight;
            }

            //calculate stuff
            pctCorrect = numPoints / numTotalPoints;
            letterGrade = CalcLetterGrade(numPoints, test.PassNum);
            string formattedAnsString = createStudentGradedAnsString(curGradedAnsString, ansKey);

            //return the results
            GradedTestData newItem = new GradedTestData();
            newItem.LetterGrade = letterGrade.ToString();
            newItem.NumCorrect = numCorrect;
            newItem.NumTotal = numTotal;
            newItem.NumPoints = numPoints;
            newItem.NumTotalPoints = numTotalPoints;
            newItem.PctCorrect = pctCorrect;
            newItem.PassNum = test.PassNum;
            newItem.CommendedNum = test.CommendedNum;
            newItem.GradedAnswers = curGradedAnsString;
            newItem.GradedAnswersFormatted = formattedAnsString;

            return newItem;
        }


        //public static void addStudentAnswerData(StGradeReportData gradeddata, string testid, string curCampus)
        //{
        //    int ansKeyVersionIncrement = new int();

        //    // go through each student and create the answer strings we need
        //    //for (int i = 0; i < gradeddata.Count; i++)
        //    foreach (StGradeReportItem item in gradeddata)
        //    {
        //        //ScanItem curScanDataItem = birIF.getLatestScanDataRowq(gradeddata.Idx(i).StudentID, gradeddata.Idx(i).TestID);
        //        if (item != null)
        //        {
        //            ansKeyVersionIncrement = ExceptionHandler.campusAnswerKeyVersionIncrement(gradeddata.Idx(i).TestID, 
        //                gradeddata.Idx(i).Campus, gradeddata.Idx(i).Teacher, gradeddata.Idx(i).Period);
        //            List<GradedItemDetail> gradeditemsdetails = GradeTestItemsInDetail(testid, curScanDataItem.Answers,
        //                curCampus, ansKeyVersionIncrement, gradeddata.Idx(i).Teacher, gradeddata.Idx(i).Period);

        //            // go through each answer and create the strings we need
        //            string curGradedAnsString = "";
        //            foreach (GradedItemDetail curgradeditem in gradeditemsdetails)
        //            {
        //                if (curgradeditem.Correct)
        //                    curGradedAnsString = curGradedAnsString + "*,";
        //                else
        //                    // ****** put either the student's answer here, or the actual correct answer ******
        //                    curGradedAnsString = curGradedAnsString + curgradeditem.StudentAnswer + ",";
        //            }
        //            curGradedAnsString = curGradedAnsString.Substring(0, curGradedAnsString.Length - 1);

        //            // make the fancy shmancy string for the report
        //            string formattedAnsString = createStudentGradedAnsString(curGradedAnsString, gradeditemsdetails);

        //            //update gradeddata 
        //            StGradeReportItem curReportItem = gradeddata.Idx(i);
        //            curReportItem.GradedAnswers = curGradedAnsString;
        //            curReportItem.GradedAnswersFormatted = formattedAnsString;
        //            gradeddata.UpdateItemAtIndexWith(i, curReportItem);
        //        }
        //    }

        //    return;
        //}






        private static string createStudentGradedAnsString(string gradedAnsString, AnswerKeyItemData ansKey)
        {
            string[] resultsString = new string[Constants.MaxFormattedAnsGroups];
            string[] titleString = new string[Constants.MaxFormattedAnsGroups];
            string formattedString = "";
            int rowIndex = 0;
            
            string[] gradedAnsStringArray = gradedAnsString.Split(',');
            int[] itemNums = ansKey.GetItems().Select(ak => ak.ItemNum).ToArray();
            Array.Sort(itemNums);

            for (int i = 0; i < gradedAnsStringArray.Length; i++)
            {
                // write this one
                if ((i + 1).ToString().Length <= 2)
                {
                    titleString[rowIndex] = titleString[rowIndex] + string.Format("{0,2} ", itemNums[i]); 
                    resultsString[rowIndex] = resultsString[rowIndex] + string.Format("{0,2} ", gradedAnsStringArray[i]);
                }
                else
                {
                    titleString[rowIndex] = titleString[rowIndex] + string.Format("{0,3} ", itemNums[i]);
                    resultsString[rowIndex] = resultsString[rowIndex] + string.Format("{0,3} ", gradedAnsStringArray[i]);
                }


                // check if the next one will fit on this row
                if ((i + 1) < gradedAnsStringArray.Length)
                {
                    if ((Constants.NumColumnsInFormattedLine - 
                        (resultsString[rowIndex].Length + itemNums[i + 1].ToString().Length)) < 2)
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




        private static char CalcLetterGrade(double pointsfor, int passnum)
        {
            if (pointsfor >= (double)passnum)
                return 'P';

                // if the pointsfor number will round up to the (int)passnum to the nearest tenth,
                //   i.e. if the difference is less than or equal to 0.05,
                //   then call this a Pass since it will display the points as such
            else if ((double)passnum - pointsfor <= (double)0.5)
                return 'P';

            return 'F';
        }
    }
}