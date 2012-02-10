using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;

namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    public class IARepHelper
    {
        /// <summary>
        /// Generate data for the Item Analysis report
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="studentData">student data to grade</param>
        /// <param name="tests">list of Test object that are included in studentData</param>
        /// <returns>IAReportData object containing data ready to display in the report</returns>
        public static IAReportData GenerateBenchmarkStatsRepTable(IRepoService dataservice,
            DataToGradeItemCollection studentData, List<Test> tests)
        {
            Hashtable finalDataH = new Hashtable();
            int curNumCorrect, curNumTotal = new int();
            AnswerCounter curAnsCount = new AnswerCounter();
            int numNull = 0;

            // go through each test
            foreach (Test curTest in tests)
            {
                
                // go through each campus for this test
                var dataWithCurTest = studentData.GetItemsWhere(d => d.TestID == curTest.TestID);
                foreach (string curSchoolAbbr in dataWithCurTest.Select(d => d.Campus).Distinct())
                {
                    
                    // go through each teacher for this test and campus
                    var dataWithCurTestSch = dataWithCurTest.Where(d => d.Campus == curSchoolAbbr);
                    foreach (string curTeacher in dataWithCurTestSch.Select(d => d.TeacherName).Distinct())
                    {
                        
                        // go through each period for this test and campus and teacher
                        var dataWithCurTestSchTch = dataWithCurTestSch.Where(d => d.TeacherName == curTeacher);
                        foreach (string curPeriod in dataWithCurTestSchTch.Select(d => d.Period).Distinct())
                        {
                            // get the current answer key and list of item numbers
                            AnswerKeyItemData curAnsKey = AnswerKeyHelper.GetTestAnswerKeyData(dataservice, curTest,
                                curSchoolAbbr, curTeacher, curPeriod);
                            int[] curItemNums = curAnsKey.GetItems().Select(k => k.ItemNum).Distinct().ToArray();
                            Array.Sort(curItemNums);
                            
                            // go through each student in the current data to grade
                            var dataWithCurTestSchTchPeriod = dataWithCurTestSchTch.Where(d => d.Period == curPeriod);
                            foreach (DataToGradeItem curStudentDataToGrade in dataWithCurTestSchTchPeriod)
                            {
                                if (curStudentDataToGrade.ScanItem.AnswerString != null)
                                {
                                    GradedTestData gradedData = GradeTests.GradeStudentAnswers(curTest,
                                        curStudentDataToGrade.ScanItem.AnswerString, curAnsKey);

                                    // go through each of the student's answers and add it to the counter
                                    foreach (int curItemNum in curItemNums)
                                    {
                                        AnswerKeyItem curAnswerKeyItem = curAnsKey.GetItemWhere(ak => ak.ItemNum == curItemNum);
                                        // see if this item is already in the dataset
                                        IAReportItem findthis = new IAReportItem(curSchoolAbbr, curTest.TestID, curTeacher, 
                                            curPeriod, curItemNum);
                                        IAReportItemKey findthiskey = new IAReportItemKey(findthis);
                                        if (finalDataH.ContainsKey(findthiskey))
                                        {
                                            // this item is here - get the values and then delete it
                                            IAReportItem foundItem = finalDataH[findthiskey] as IAReportItem;
                                            curNumCorrect = foundItem.NumCorrect;
                                            curNumTotal = foundItem.NumTotal;
                                            curAnsCount.UpdateFromReportItem(foundItem);

                                            finalDataH.Remove(findthiskey);                            
                                        }
                                        else
                                        {
                                            curNumCorrect = 0;
                                            curNumTotal = 0;
                                            curAnsCount.Reset();
                                        }

                                        // add the row to the results table
                                        IAReportItem newItem = new IAReportItem();
                                        newItem.Campus = curSchoolAbbr;
                                        newItem.TestID = curTest.TestID;
                                        newItem.Teacher = curTeacher;
                                        newItem.Period = curPeriod;
                                        newItem.ItemNum = curItemNum;
                                        if (gradedData.ResponsesCorrect.Single(rc => rc.ItemNum == curItemNum).Info)
                                            curNumCorrect++;
                                        curAnsCount.Increment(gradedData.Responses.Single(r => r.ItemNum == curItemNum).Info);
                                        curNumTotal++;
                                        newItem.NumCorrect = curNumCorrect;
                                        newItem.NumTotal = curNumTotal;
                                        newItem.PctCorrect = (decimal)curNumCorrect / (decimal)curNumTotal;
                                        curAnsCount.UpdateToReportItem(newItem);
                                        newItem.Answer = curAnswerKeyItem.Answer;
                                        newItem.Objective = curAnswerKeyItem.Category;
                                        newItem.TEKS = curAnswerKeyItem.TEKS;

                                        IAReportItemKey newItemKey = new IAReportItemKey(newItem);
                                        finalDataH.Add(newItemKey, newItem);
                                    }
                                }
                                else
                                {
                                    numNull++;
                                }

                                } // (foreach student)

                            } // (foreach period)
                    
                        } // (foreach teacher)
                
                    } // (foreach school)
             
                } // (foreach Test)

            return new IAReportData(finalDataH.Values.Cast<IAReportItem>().ToList());
        }

    }
}