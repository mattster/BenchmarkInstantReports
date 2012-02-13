using System;
using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Exceptions;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class GradeTests
    {

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
            if (GridHandler.isGriddable(test))
                GridHandler.ProcessAnswerStringWithGrids(studentAnswerStringArray, test);

            if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(test))
                MultiAnswerTemplateHandler.ProcessAnswerStringWithMultiAnswers(studentAnswerStringArray, test);

            // grade each item on the test
            numCorrect = 0;
            numPoints = 0;
            numTotalPoints = 0;
            numTotal = ansKey.Count;
            //string curGradedAnsString = "";
            var gradedAnswers = new List<ItemInfo<string>>();
            var responses = new List<ItemInfo<string>>();
            var responsescorrect = new List<ItemInfo<bool>>();
            foreach (AnswerKeyItem itemAnswerKey in ansKey.GetItems())
            {
                curItemNum = itemAnswerKey.ItemNum;
                string studentAnswer = studentAnswerStringArray[curItemNum - 1];
                responses.Add(new ItemInfo<string>(curItemNum, studentAnswer));

                if (studentAnswerStringArray.Length >= curItemNum)       // a student answer exists
                {
                    if (studentAnswer == itemAnswerKey.Answer)
                    {
                        // student's answer is correct
                        numCorrect++;
                        numPoints += itemAnswerKey.Weight;
                        responsescorrect.Add(new ItemInfo<bool>(curItemNum, true));
                        gradedAnswers.Add(new ItemInfo<string>(curItemNum, Constants.CorrectAnswerIndicator));
                        //curGradedAnsString += Constants.CorrectAnswerIndicator + ",";
                    }
                    else if (studentAnswer ==
                        ExceptionHandler.getAlternateAnswer(test.TestID, curItemNum, itemAnswerKey.Campus))
                    {
                        // student's answer is correct as the alternate answer
                        numCorrect++;
                        numPoints += itemAnswerKey.Weight;
                        responsescorrect.Add(new ItemInfo<bool>(curItemNum, true));
                        gradedAnswers.Add(new ItemInfo<string>(curItemNum, Constants.CorrectAnswerIndicator));
                        //curGradedAnsString = curGradedAnsString + Constants.CorrectAnswerIndicator + ",";
                    }
                    else
                    {
                        // student's answer is incorrect
                        responsescorrect.Add(new ItemInfo<bool>(curItemNum, false));
                        gradedAnswers.Add(new ItemInfo<string>(curItemNum, studentAnswer));
                        //curGradedAnsString = curGradedAnsString + studentAnswer + ",";
                    }
                }

                numTotalPoints += itemAnswerKey.Weight;
            }
            //curGradedAnsString = curGradedAnsString.Substring(0, curGradedAnsString.Length - 1);

            //calculate stuff
            pctCorrect = numPoints / numTotalPoints;
            letterGrade = CalcLetterGrade(numPoints, test.PassNum);
            string formattedAnsString = GradeFormatting.CreateStudentSummaryDisplayField(gradedAnswers, ansKey);

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
            newItem.Responses = responses;
            newItem.ResponsesCorrect = responsescorrect;
            newItem.GradedAnswers = string.Join(",", gradedAnswers.Select(ga => ga.Info).ToArray());
            newItem.GradedAnswersFormatted = formattedAnsString;

            return newItem;
        }





        ///// <summary>
        ///// creates a formatted answer string containing item numbers and student responses
        ///// for use in the Student Summary report
        ///// </summary>
        ///// <param name="gradedAnsString">a comma-separated string of just student responses:
        ///// CorrectAnswerIndicator if student's response was correct, the actual response otherwise</param>
        ///// <param name="ansKey">the answer key for this test</param>
        ///// <returns>a formatted string with newlines for displaying in the Student Summary report</returns>
        //private static string createStudentGradedAnsString(string gradedAnsString, AnswerKeyItemData ansKey)
        //{
        //    string[] resultsString = new string[Constants.MaxFormattedAnsGroups];
        //    string[] titleString = new string[Constants.MaxFormattedAnsGroups];
        //    string formattedString = "";
        //    int rowIndex = 0;
            
        //    string[] gradedAnsStringArray = gradedAnsString.Split(',');
        //    int[] itemNums = ansKey.GetItems().Select(ak => ak.ItemNum).ToArray();
        //    Array.Sort(itemNums);

        //    for (int i = 0; i < gradedAnsStringArray.Length; i++)
        //    {
        //        // write this one
        //        if ((i + 1).ToString().Length <= 2)
        //        {
        //            titleString[rowIndex] = titleString[rowIndex] + string.Format("{0,2} ", itemNums[i]); 
        //            resultsString[rowIndex] = resultsString[rowIndex] + string.Format("{0,2} ", gradedAnsStringArray[i]);
        //        }
        //        else
        //        {
        //            titleString[rowIndex] = titleString[rowIndex] + string.Format("{0,3} ", itemNums[i]);
        //            resultsString[rowIndex] = resultsString[rowIndex] + string.Format("{0,3} ", gradedAnsStringArray[i]);
        //        }


        //        // check if the next one will fit on this row
        //        if ((i + 1) < gradedAnsStringArray.Length)
        //        {
        //            if ((Constants.NumColumnsInFormattedLine - 
        //                (resultsString[rowIndex].Length + itemNums[i + 1].ToString().Length)) < 2)
        //            {
        //                // next item will not fit; delete extra space at end and go to next row
        //                titleString[rowIndex] = titleString[rowIndex].Substring(0, titleString[rowIndex].Length - 1);
        //                resultsString[rowIndex] = resultsString[rowIndex].Substring(0, resultsString[rowIndex].Length - 1);
        //                rowIndex++;
        //            }
        //        }
        //    }

        //    // put the rows together, insert newlines
        //    for (int j = 0; j <= rowIndex; j++)
        //    {
        //        formattedString = formattedString + String.Format("{0}\n{1}\n", titleString[j], resultsString[j]);
        //    }

        //    // remove the last newline - we don't need it
        //    formattedString = formattedString.Substring(0, formattedString.Length - 1);

        //    return formattedString;
        //}



        /// <summary>
        /// calculates a letter grade for a student based on the current number of points
        /// </summary>
        /// <param name="pointsfor">number of points the student earned</param>
        /// <param name="passnum">number of points for the passing cutoff</param>
        /// <returns>'P' if the student passed, 'F' otherwise</returns>
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