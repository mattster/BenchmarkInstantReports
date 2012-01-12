﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using System.Data;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Helpers.Reports;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class StudentData
    {
        public static List<StudentScanDataItem> GetStudentDataToGradeq(string testID, string campus, string teacher = "",
            string periodList = "'00','01','02','03','04','05','06','07','08','09','10','11','12','13','14'")
        {
            //DataSet dsReturn = new DataSet();
            List<string> studIdList1 = new List<string>();

            // get a dataset of the student scans for this test and campus
            string qs1 = Queries.GetScansForCampus.Replace("@campus", campus);
            qs1 = qs1.Replace("@testId", testID);
            DataSet dsStudentScansForCampus = dbIFOracle.getDataRows(qs1);

            // get a dataset of students who meet the criteria for this test
            string qs2 = Queries.GetCustomQuery.Replace("@testId", testID);
            DataSet dsCustQuery = dbIFOracle.getDataRows(qs2);
            string qs3 = dsCustQuery.Tables[0].Rows[0][0].ToString().Replace("@school", "\'" + campus + "\'");
            DataSet dsPreslugDataForTest = dbIFOracle.getDataRows(qs3);
            string[] teacherList = birUtilities.getUniqueTableColumnStringValues(dsPreslugDataForTest.Tables[0], Constants.TeacherNameFieldName);

            // get student data for students who have scans and meet the test criteria
            DataSet dsPresluggedStudentsWithScans = new DataSet();
            List<StudentScanDataItem> PresluggedStudentsWithScans = new List<StudentScanDataItem>();
            if (teacher == "")
                //dsPresluggedStudentsWithScans = getStudentScanListData2(benchmark, campus);
                PresluggedStudentsWithScans = birIF.getStudentScanListData2q(testID, campus);
            else
                //dsPresluggedStudentsWithScans = getStudentScanListData(benchmark, campus, teacher, periodList);
                PresluggedStudentsWithScans = birIF.getStudentScanListDataq(testID, campus, teacher, periodList);
            //DataTable returnTable = dsPresluggedStudentsWithScans.Tables[0].Copy();
            List<StudentScanDataItem> returnData = PresluggedStudentsWithScans;

            // get student ID's for students who have scans but do not meet the test criteria
            string qs4 = Queries.GetStudentsWithScansNotInTestCriteria.Replace("@testId", testID);
            qs4 = qs4.Replace("@campus", campus);
            string customQuery = birIF.GetRawCustomQuery(testID);
            customQuery = customQuery.Replace("@school", "\'" + campus + "\'");
            qs4 = qs4.Replace("@query", customQuery);
            DataSet dsStudentsWithScansNotInTestCriteria = dbIFOracle.getDataRows(qs4);

            // for any students who do not match test criteria, try to match them up somehow
            if (dsStudentsWithScansNotInTestCriteria.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsStudentsWithScansNotInTestCriteria.Tables[0].Rows)
                {
                    string studentId = row["STUDENT_ID"].ToString();
                    if (studentId.Length == 5)              // add a leading 0 if necessary - makes things better
                        studentId = "0" + studentId;

                    if (birUtilities.isTeacherUnknown(studentId))
                    {
                        //DataRow temprow = returnTable.NewRow();
                        StudentScanDataItem newItem = new StudentScanDataItem();
                        //temprow["LOCAL_STUDENT_ID"] = studentId;
                        newItem.StudentID = studentId;
                        //temprow["STUDENT_NAME"] = "";
                        newItem.StudentName = "";
                        //temprow[Constants.TeacherNameFieldName] = Constants.UnknownTeacherName;
                        newItem.TeacherName = Constants.UnknownTeacherName;
                        //temprow["PERIOD"] = "01";
                        newItem.Period = "01";
                        //temprow["LOCAL_COURSE_ID"] = "00000";
                        newItem.CourseID = "00000";
                        //temprow["SCHOOL2"] = campus;
                        newItem.Campus = campus;
                        //temprow["TEST_ID"] = benchmark;
                        newItem.TestID = testID;
                        //returnTable.Rows.Add(temprow);
                        returnData.Add(newItem);
                    }
                    else
                    {

                        // try to match them to teachers in the test query
                        string qs = Queries.MatchStudentToTeacherList.Replace("@testId", testID);
                        qs = qs.Replace("@studentId", studentId);
                        qs = qs.Replace("@teacherList", birUtilities.convertStringArrayForQuery(teacherList));
                        //DataSet dsStudentMatches = dbIFOracle.getDataRows(qs);
                        List<StudentScanDataItem> StudentMatches = DBIOWorkaround.ReturnStudentScanDataItemsFromQ(qs);
                        //if (dsStudentMatches.Tables[0].Rows.Count > 0)
                        if (StudentMatches.Count > 0)
                        {
                            //DataRow temprow = returnTable.NewRow();
                            StudentScanDataItem newItem = new StudentScanDataItem();
                            //temprow.ItemArray = dsStudentMatches.Tables[0].Rows[0].ItemArray;
                            newItem = StudentMatches[0];
                            //returnTable.Rows.Add(temprow);
                            returnData.Add(newItem);
                        }

                        else
                        {
                            // try to match them up to teachers when removing the BENCHMARK_MOD criteria
                            string customQueryNoMod = customQuery.Replace("AND BENCHMARK_MOD LIKE \'____1\'", " ");
                            customQueryNoMod = customQueryNoMod.Replace("AND BENCHMARK_MOD LIKE \'___1_\'", " ");
                            customQueryNoMod = customQueryNoMod.Replace("AND BENCHMARK_MOD LIKE \'__1__\'", " ");
                            customQueryNoMod = customQueryNoMod.Replace("AND BENCHMARK_MOD LIKE \'_1___\'", " ");
                            customQueryNoMod = customQueryNoMod.Replace("AND BENCHMARK_MOD LIKE \'1____\'", " ");
                            //DataSet dsStudentMatches = dbIFOracle.getDataRows(customQueryNoMod);
                            List<StudentScanDataItem> StudentMatches2 = DBIOWorkaround.ReturnStudentScanDataItemsFromQ(customQueryNoMod);
                            //if (dsStudentMatches.Tables[0].Rows.Count > 0)
                            if (StudentMatches2.Count > 0)
                            {
                                string selectStudentFilter = "LOCAL_STUDENT_ID = \'" + studentId + "\'";
                                //DataRow[] matchingRows = dsStudentMatches.Tables[0].Select(selectStudentFilter);
                                List<StudentScanDataItem> matchingData =
                                    StudentMatches2.FindAll(delegate(StudentScanDataItem item)
                                    {
                                        return item.StudentID == studentId;
                                    });
                                //int c = matchingRows.Count();
                                int c = matchingData.Count();
                                //if (matchingRows.Count() > 0)
                                if (matchingData.Count() > 0)
                                {
                                    //DataRow temprow = returnTable.NewRow();
                                    StudentScanDataItem newItem = new StudentScanDataItem();
                                    //temprow["LOCAL_STUDENT_ID"] = matchingRows[0]["LOCAL_STUDENT_ID"];
                                    newItem.StudentID = matchingData[0].StudentID;
                                    //temprow["STUDENT_NAME"] = matchingRows[0]["STUDENT_NAME"];
                                    newItem.StudentName = matchingData[0].StudentName;
                                    //temprow[Constants.TeacherNameFieldName] = matchingRows[0][Constants.TeacherNameFieldName];
                                    newItem.TeacherName = matchingData[0].TeacherName;
                                    //temprow["PERIOD"] = matchingRows[0]["PERIOD"];
                                    newItem.Period = matchingData[0].Period;
                                    //temprow["LOCAL_COURSE_ID"] = matchingRows[0]["LOCAL_COURSE_ID"];
                                    newItem.CourseID = matchingData[0].CourseID;
                                    //temprow["SCHOOL2"] = matchingRows[0]["SCHOOL2"];
                                    newItem.Campus = matchingData[0].Campus;
                                    //temprow["TEST_ID"] = benchmark;
                                    newItem.TestID = testID;
                                    //returnTable.Rows.Add(temprow);
                                    returnData.Add(newItem);
                                }

                                else
                                {
                                    // no matching teacher found in query removing BENCHMARK_MOD criteria, put "UNKNOWN"
                                    //DataRow temprow = returnTable.NewRow();
                                    StudentScanDataItem newItem = new StudentScanDataItem();
                                    //temprow["LOCAL_STUDENT_ID"] = studentId;
                                    newItem.StudentID = studentId;
                                    //temprow["STUDENT_NAME"] = "";
                                    //temprow[Constants.TeacherNameFieldName] = Constants.UnknownTeacherName;
                                    newItem.TeacherName = Constants.UnknownTeacherName;
                                    //temprow["PERIOD"] = "01";
                                    newItem.Period = "01";
                                    //temprow["LOCAL_COURSE_ID"] = "00000";
                                    newItem.CourseID = "00000";
                                    //temprow["SCHOOL2"] = campus;
                                    newItem.Campus = campus;
                                    //temprow["TEST_ID"] = benchmark;
                                    newItem.TestID = testID;
                                    //returnTable.Rows.Add(temprow);
                                    returnData.Add(newItem);
                                }
                            }

                            else
                            {
                                // no rows found when removing BENCHMARK_MOD criteria, put "UNKNOWN"
                                //DataRow temprow = returnTable.NewRow();
                                StudentScanDataItem newItem = new StudentScanDataItem();
                                //temprow["LOCAL_STUDENT_ID"] = studentId;
                                newItem.StudentID = studentId;
                                //temprow["STUDENT_NAME"] = "";
                                //temprow[Constants.TeacherNameFieldName] = Constants.UnknownTeacherName;
                                newItem.TeacherName = Constants.UnknownTeacherName;
                                //temprow["PERIOD"] = "01";
                                newItem.Period = "01";
                                //temprow["LOCAL_COURSE_ID"] = "00000";
                                newItem.CourseID = "00000";
                                //temprow["SCOHOL_ABBR"] = campus;
                                newItem.Campus = campus;
                                //temprow["TEST_ID"] = benchmark;
                                newItem.TestID = testID;
                                //returnTable.Rows.Add(temprow);
                                returnData.Add(newItem);
                            }
                        }
                    }
                }
            }

            //dsReturn.Tables.Add(returnTable);

            //return dsReturn;
            return returnData;
        }

        //public static ResultsTableData generateBenchmarkStatsRepTableQ(List<StudentScanDataItem> studentData, string curTest, string curCampus)
        //{
        //    string curId;
        //    int curNumCorrect, curNumTotal = new int();
        //    AnswerCounter curAnsCount = new AnswerCounter();

        //    ResultsTableData finalData = new ResultsTableData();

        //    // grade each student's test and add it to the DataSet
        //    int numNull = 0;
        //    for (int jj = 0; jj < studentData.Count; jj++)
        //    {
        //        curId = studentData[jj].StudentID;
                
        //        ScanItem curScanItem = birIF.getLatestScanDataRowq(curId, curTest);
        //        if (curScanItem != null)
        //        {
        //            List<GradedItemDetail> gradedData = GradeTest.gradeScannedTestDetailQ(curTest, curScanItem.Answers, curCampus, 0,
        //                studentData[jj].TeacherName, studentData[jj].Period);
                    
        //            // add data for each test item to the resultsTable
        //            for (int k = 0; k < gradedData.Count; k++)
        //            {
        //                // see if this item is already in the dataset
        //                ResultsTableData foundData = new ResultsTableData(finalData.GetItemsWhere( i => 
        //                        i.Campus == studentData[jj].Campus &&    
        //                        i.TestID == curTest &&
        //                        i.Teacher == studentData[jj].TeacherName.Replace("'", "''") &&
        //                        i.Period == studentData[jj].Period &&
        //                        i.ItemNum == gradedData[k].ItemNum));

        //                if (foundData.Count > 0)
        //                {
        //                    // this item is here - get the values and then delete it
        //                    curNumCorrect = foundData.First().NumCorrect;
        //                    curNumTotal = foundData.First().NumTotal;
        //                    curAnsCount.UpdateFromResultsTableItem(foundData.First());

        //                    finalData.Remove(finalData.GetItemWhere(i =>
        //                            i.Campus == foundData.First().Campus &&
        //                            i.TestID == foundData.First().TestID &&
        //                            i.Teacher == foundData.First().Teacher &&
        //                            i.Period == foundData.First().Period &&
        //                            i.ItemNum == foundData.First().ItemNum));
        //                }
        //                else
        //                {
        //                    curNumCorrect = 0;
        //                    curNumTotal = 0;
        //                    curAnsCount.Reset();
        //                }

        //                // add the row to the results table
        //                ResultsTableItem newItem = new ResultsTableItem();
        //                newItem.Campus = studentData[jj].Campus;
        //                newItem.TestID = curTest;
        //                newItem.Teacher = studentData[jj].TeacherName;
        //                newItem.Period = studentData[jj].Period;
        //                newItem.ItemNum = gradedData[k].ItemNum;
        //                if (gradedData[k].Correct)
        //                    curNumCorrect++;
        //                curAnsCount.Increment(gradedData[k].StudentAnswer);
        //                curNumTotal++;
        //                newItem.NumCorrect = curNumCorrect;
        //                newItem.NumTotal = curNumTotal;
        //                newItem.PctCorrect = (decimal)curNumCorrect / (decimal)curNumTotal; 
        //                curAnsCount.UpdateToResultsTableItem(newItem);
        //                newItem.Answer = gradedData[k].CorrectAnswer;
        //                newItem.Objective = gradedData[k].Category;
        //                newItem.TEKS = gradedData[k].TEKS;

        //                finalData.Add(newItem);
        //            }

        //        }
        //        else
        //        {
        //            numNull++;
        //        }
        //    }

        //    return finalData;
        //}
    
    
    }
}