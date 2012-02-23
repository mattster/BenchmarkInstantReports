using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Exceptions;
using Benchmark_Instant_Reports_2.Infrastructure;
using Benchmark_Instant_Reports_2.Infrastructure.DataStruct;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.References;

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
                        if (studentAnswer == " " || studentAnswer == "")
                            studentAnswer = Constants.BlankAnswerIndicator;
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
            //string formattedAnsString = GradeFormatting.CreateStudentSummaryDisplayField(gradedAnswers, ansKey);
            string formattedAnsString = GradeFormatting.createStudentGradedAnsString(gradedAnswers, ansKey);

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





        #region private

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

        #endregion
    }
}