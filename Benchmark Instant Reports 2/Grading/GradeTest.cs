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
        public static List<GradedItemDetail> gradeScannedTestDetailQ(string testID, string studentAnswerString, string curCampus, int ansKeyIncAmt, string teacher, string period)
        {
            //DataSet dsAnswerKey = new DataSet();
            //DataView dvAnswerKey = new DataView();
            //string[] columnLabels = { "ITEM_NUM", "CORRECT", "CORRECT_ANS", "STUDENT_ANS", "OBJECTIVE", "TEKS" };
            int itemObjective = new int();
            int curItemNum = new int();
            //int thisTestTemplType;

            //get the test template type for use in calculating griddables
            //thisTestTemplType = getTestTemplateType(testID);

            //convert answer string to an array
            string[] studentAnswerStringArray = studentAnswerString.Split(',');

            // do special processing for special template types
            if (GridHandler.isGriddable(testID))
                GridHandler.ProcessAnswerStringWithGrids(studentAnswerStringArray, testID, curCampus);

            if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(testID))
                MultiAnswerTemplateHandler.ProcessAnswerStringWithMultiAnswers(studentAnswerStringArray, testID, curCampus);

            //get the answer key as a DataSet
            //dsAnswerKey = getTestAnswerKey(testID, curCampus, ansKeyIncAmt, teacher, period);
            List<AnswerKeyItem> theAnswerKey = AnswerKey.getTestAnswerKeyQ(testID, curCampus, ansKeyIncAmt, teacher, period);
            //dvAnswerKey = new DataView(dsAnswerKey.Tables[0]);


            //DataTable table = new DataTable();
            List<GradedItemDetail> finalData = new List<GradedItemDetail>();
            //table.Columns.Add(columnLabels[0], System.Type.GetType("System.Int32"));
            //table.Columns.Add(columnLabels[1], System.Type.GetType("System.Boolean"));
            //table.Columns.Add(columnLabels[2], System.Type.GetType("System.String"));
            //table.Columns.Add(columnLabels[3], System.Type.GetType("System.String"));
            //table.Columns.Add(columnLabels[4], System.Type.GetType("System.Int32"));
            //table.Columns.Add(columnLabels[5], System.Type.GetType("System.String"));

            //grade each item on the test
            //int numTotal = dvAnswerKey.Table.Rows.Count;
            int numTotal = theAnswerKey.Count;
            for (int i = 0; i < numTotal; i++)
            {
                //string correctAnswer = dvAnswerKey.Table.Rows[i]["ANSWER"].ToString();
                string correctAnswer = theAnswerKey[i].Answer;
                //itemObjective = Convert.ToInt32(dvAnswerKey.Table.Rows[i]["OBJECTIVE"].ToString());
                itemObjective = theAnswerKey[i].Category;
                //string itemTEKS = dvAnswerKey.Table.Rows[i]["TEKS"].ToString();
                string itemTEKS = theAnswerKey[i].TEKS;
                //curItemNum = (int)(decimal)dvAnswerKey.Table.Rows[i]["ITEM_NUM"];
                curItemNum = theAnswerKey[i].ItemNum;

                //DataRow row = table.NewRow();
                GradedItemDetail newItem = new GradedItemDetail();
                //row[columnLabels[0]] = curItemNum;                      // ITEM_NUM
                newItem.ItemNum = curItemNum;
                //row[columnLabels[2]] = correctAnswer;                   // write the correct answer
                newItem.CorrectAnswer = correctAnswer;
                //row[columnLabels[4]] = itemObjective;                   // the Objective number
                newItem.Category = itemObjective;
                //row[columnLabels[5]] = itemTEKS;                        // the TEKS code
                newItem.TEKS = itemTEKS;


                if (studentAnswerStringArray.Length >= curItemNum)
                {                                                       // a student answer exists
                    string studentAnswer = studentAnswerStringArray[curItemNum - 1];
                    studentAnswer = (studentAnswer.Trim() == "") ? "-" : studentAnswer;
                    //row[columnLabels[3]] = studentAnswer;
                    newItem.StudentAnswer = studentAnswer;
                    
                    if (studentAnswer == correctAnswer)
                    {                                                   // correct answer
                        //row[columnLabels[1]] = true;
                        newItem.Correct = true;
                    }
                    else if (studentAnswer == ExceptionHandler.getAlternateAnswer(testID, curItemNum, curCampus))
                    {
                        //row[columnLabels[1]] = true;
                        newItem.Correct = true;
                    }
                    else
                    {                                                   // incorrect answer
                        //row[columnLabels[1]] = false;
                        newItem.Correct = false;
                    }
                }
                else
                {                                                       // no student answer
                    //row[columnLabels[3]] = "";
                    //newItem.StudentAnswer = "";
                    //row[columnLabels[1]] = false;
                    newItem.Correct = false;
                }
                //table.Rows.Add(row);
                finalData.Add(newItem);
            }

            //return table;
            return finalData;

        }


        public static List<GradedItem> gradeScannedTestQ(string testID, string studentAnswerString, string curCampus, int ansKeyIncAmt, string teacher, string period)
        {
            //DataSet dsAnswerKey = new DataSet();
            //DataView dvAnswerKey = new DataView();
            int numCorrect, numTotal = new int();
            int curItemNum = new int();
            decimal pctCorrect = new decimal();
            char letterGrade = new char();
            //string[] columnLabels = { "LETTER_GRADE", "NUM_CORRECT", "NUM_TOTAL", "PCT_CORRECT", 
            //                          "PASS_NUM", "COMMENDED_NUM" };
            //int thisTestTemplType;

            //get pass and commended numbers for this test
            int passNum = birIF.getTestPassingNum(testID);
            int commendedNum = birIF.getTestCommendedNum(testID);

            //get the test template type for use in calculating griddables
            //thisTestTemplType = getTestTemplateType(testID);

            //convert answer string to an array
            string[] studentAnswerStringArray = studentAnswerString.Split(',');

            // do special processing for special template types
            if (GridHandler.isGriddable(testID))
                GridHandler.ProcessAnswerStringWithGrids(studentAnswerStringArray, testID, curCampus);

            if (MultiAnswerTemplateHandler.IsMultiAnswerTemplate(testID))
                MultiAnswerTemplateHandler.ProcessAnswerStringWithMultiAnswers(studentAnswerStringArray, testID, curCampus);

            //get the answer key as a DataSet
            //dsAnswerKey = getTestAnswerKey(testID, curCampus, ansKeyIncAmt, teacher, period);
            //dvAnswerKey = new DataView(dsAnswerKey.Tables[0]);
            List<AnswerKeyItem> theAnswerKey = AnswerKey.getTestAnswerKeyQ(testID, curCampus, ansKeyIncAmt, teacher, period);

            //grade each item on the test
            numCorrect = 0;
            //numTotal = dvAnswerKey.Table.Rows.Count;
            numTotal = theAnswerKey.Count;

            for (int i = 0; i < numTotal; i++)
            {
                //curItemNum = (int)(decimal)dvAnswerKey.Table.Rows[i]["ITEM_NUM"];
                curItemNum = theAnswerKey[i].ItemNum;


                if (studentAnswerStringArray.Length >= curItemNum)       // a student answer exists
                {
                    //if (studentAnswerStringArray[curItemNum - 1] == dvAnswerKey.Table.Rows[i]["ANSWER"].ToString())
                    if (studentAnswerStringArray[curItemNum - 1] == theAnswerKey[i].Answer)
                    {
                        // student's answer is correct
                        numCorrect++;
                    }
                    if (studentAnswerStringArray[curItemNum - 1] == ExceptionHandler.getAlternateAnswer(testID, curItemNum, curCampus))
                    {
                        // student's answer is correct as the alternate answer
                        numCorrect++;
                    }
                }
            }

            //calculate stuff
            pctCorrect = (decimal)numCorrect / (decimal)numTotal;
            letterGrade = birUtilities.calcLetterGrade(testID, numCorrect, numTotal, passNum, commendedNum);

            //return the results
            //DataTable table = new DataTable();
            List<GradedItem> finalData = new List<GradedItem>();
            //table.Columns.Add(columnLabels[0], System.Type.GetType("System.Char"));
            //table.Columns.Add(columnLabels[1], System.Type.GetType("System.Int32"));
            //table.Columns.Add(columnLabels[2], System.Type.GetType("System.Int32"));
            //table.Columns.Add(columnLabels[3], System.Type.GetType("System.Decimal"));
            //table.Columns.Add(columnLabels[4], System.Type.GetType("System.Int32"));
            //table.Columns.Add(columnLabels[5], System.Type.GetType("System.Int32"));
            //DataRow row = table.NewRow();
            GradedItem newItem = new GradedItem();
            //row[columnLabels[0]] = letterGrade;                 // letter grade
            newItem.LetterGrade = letterGrade.ToString();
            //row[columnLabels[1]] = numCorrect;                  // num correct
            newItem.NumCorrect = numCorrect;
            //row[columnLabels[2]] = numTotal;                    // num total
            newItem.NumTotal = numTotal;
            //row[columnLabels[3]] = pctCorrect;                  // pct correct
            newItem.PctCorrect = pctCorrect;
            //row[columnLabels[4]] = passNum;                     // pass num
            newItem.PassNum = passNum;
            //row[columnLabels[5]] = commendedNum;                // commended num
            newItem.CommendedNum = commendedNum;
            //table.Rows.Add(row);
            finalData.Add(newItem);

            //return table;
            return finalData;
        }

    }
}