using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Exceptions;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.Grading;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class GradeTest
    {
        public static List<GradedItemDetail> gradeScannedTestDetailQ(string testID, string studentAnswerString, string curCampus, 
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

            List<AnswerKeyItem> theAnswerKey = AnswerKey.getTestAnswerKeyQ(testID, curCampus, ansKeyIncAmt, teacher, period);

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


        public static GradedItem gradeScannedTestQ(string testID, string studentAnswerString, string curCampus, int ansKeyIncAmt, 
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

            List<AnswerKeyItem> theAnswerKey = AnswerKey.getTestAnswerKeyQ(testID, curCampus, ansKeyIncAmt, teacher, period);

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

                    numTotalPoints += itemAnswerKey.Weight;
                }
            }

            //calculate stuff
            //pctCorrect = (decimal)numCorrect / (decimal)numTotal;
            pctCorrect = numPoints / numTotalPoints;
            letterGrade = birUtilities.calcLetterGrade(testID, numCorrect, numTotal, passNum, commendedNum);

            //return the results
            GradedItem newItem = new GradedItem();
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

    }
}