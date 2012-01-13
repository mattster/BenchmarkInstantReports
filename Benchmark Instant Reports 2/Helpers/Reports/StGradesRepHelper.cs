using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Exceptions;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;


namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    public class StGradesRepHelper
    {
        public static StGradeReportData generateStudentStatsRepTable(List<StudentListItem> studentData, string curTest)
        {
            string curId;
            StGradeReportData finalData = new StGradeReportData();
            Hashtable resultsDataH = new Hashtable();
            int ansKeyVersionIncrement = new int();

            // grade each student's test and add it to the DataSet
            int numNull = 0;
            for (int j = 0; j < studentData.Count; j++)
            {
                curId = studentData[j].StudentID;
                string curCampus = studentData[j].Campus;
                ScanItem curScanDataItem = birIF.getLatestScanDataRowq(curId, curTest);
                if (curScanDataItem != null)
                {
                    ansKeyVersionIncrement = ExceptionHandler.campusAnswerKeyVersionIncrement(curTest, curCampus,
                        studentData[j].TeacherName, studentData[j].Period);

                    List<GradedItem> gradedData = GradeTest.gradeScannedTestQ(curTest, curScanDataItem.Answers, curCampus, ansKeyVersionIncrement,
                        studentData[j].TeacherName, studentData[j].Period);

                    StGradeReportItem newItem = new StGradeReportItem();
                    newItem.StudentID = studentData[j].StudentID;
                    newItem.StudentName = studentData[j].StudentName;
                    newItem.TestID = studentData[j].TestID;
                    newItem.ScanDate = DateTime.Parse(curScanDataItem.DateScannedStr);
                    newItem.Campus = studentData[j].Campus;
                    newItem.Teacher = studentData[j].TeacherName;
                    newItem.Period = studentData[j].Period;

                    newItem.LetterGrade = gradedData[0].LetterGrade;
                    newItem.NumCorrect = gradedData[0].NumCorrect;
                    newItem.NumTotal = gradedData[0].NumTotal;
                    newItem.PctCorrect = gradedData[0].PctCorrect;
                    newItem.PassNum = gradedData[0].PassNum;
                    newItem.CommendedNum = gradedData[0].CommendedNum;
                    
                    StGradeReportItemKey newItemKey = new StGradeReportItemKey(newItem);
                    resultsDataH.Add(newItemKey, newItem);
                }
                else
                {
                    numNull++;
                }
            }

            return new StGradeReportData(resultsDataH.Values.Cast<StGradeReportItem>().ToList());
        }
    }
}