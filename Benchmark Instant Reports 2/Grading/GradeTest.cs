using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class GradeTest
    {
        public static List<ResultsTableItem> gradeScannedTestDetail(string testID, string studentAnswerString, string curCampus, int ansKeyIncAmt, string teacher, string period)
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
            List<AnswerKey> theAnswerKey = getTestAnswerKey(testID, curCampus, ansKeyIncAmt, teacher, period);
            //dvAnswerKey = new DataView(dsAnswerKey.Tables[0]);


            //DataTable table = new DataTable();
            List<ResultsTableItem> finalData = new List<ResultsTableItem>();
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
                ResultsTableItem newItem = new ResultsTableItem();
                //row[columnLabels[0]] = curItemNum;                      // ITEM_NUM
                //row[columnLabels[2]] = correctAnswer;                   // write the correct answer
                //row[columnLabels[4]] = itemObjective;                   // the Objective number
                //row[columnLabels[5]] = itemTEKS;                        // the TEKS code


                if (studentAnswerStringArray.Length >= curItemNum)
                {                                                       // a student answer exists
                    string studentAnswer = studentAnswerStringArray[curItemNum - 1];
                    studentAnswer = (studentAnswer.Trim() == "") ? "-" : studentAnswer;
                    row[columnLabels[3]] = studentAnswer;
                    
                    if (studentAnswer == correctAnswer)
                    {                                                   // correct answer
                        row[columnLabels[1]] = true;
                    }
                    else if (studentAnswer == ExceptionHandler.getAlternateAnswer(testID, curItemNum, curCampus))
                    {
                        row[columnLabels[1]] = true;
                    }
                    else
                    {                                                   // incorrect answer
                        row[columnLabels[1]] = false;
                    }
                }
                else
                {                                                       // no student answer
                    row[columnLabels[3]] = "";
                    row[columnLabels[1]] = false;
                }
                table.Rows.Add(row);
            }

            return table;

        }

    
    }
}