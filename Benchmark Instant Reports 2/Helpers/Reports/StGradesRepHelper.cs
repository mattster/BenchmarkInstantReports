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
            int ansKeyVersionIncrement = new int();

            // grade each student's test and add it to the DataSet
            int numNull = 0;
            foreach (StudentListItem item in studentData)
            {
                curId = item.StudentID;
                string curCampus = item.Campus;
                ScanItem curScanDataItem = birIF.getLatestScanDataRowq(curId, curTest);
                if (curScanDataItem != null)
                {
                    ansKeyVersionIncrement = ExceptionHandler.campusAnswerKeyVersionIncrement(curTest, curCampus,
                        item.TeacherName, item.Period);

                    GradedItem gradedData = GradeTest.gradeScannedTestQ(curTest, curScanDataItem.Answers, curCampus, ansKeyVersionIncrement,
                        item.TeacherName, item.Period);

                    StGradeReportItem newItem = new StGradeReportItem();
                    newItem.StudentID = item.StudentID;
                    newItem.StudentName = item.StudentName;
                    newItem.TestID = item.TestID;
                    newItem.ScanDate = DateTime.Parse(curScanDataItem.DateScannedStr);
                    newItem.Campus = item.Campus;
                    newItem.Teacher = item.TeacherName;
                    newItem.Period = item.Period;

                    newItem.LetterGrade = gradedData.LetterGrade;
                    newItem.NumCorrect = gradedData.NumCorrect;
                    newItem.NumTotal = gradedData.NumTotal;
                    newItem.NumPoints = gradedData.NumPoints;
                    newItem.NumTotalPoints = gradedData.NumTotalPoints;
                    newItem.PctCorrect = gradedData.PctCorrect;
                    newItem.PassNum = gradedData.PassNum;
                    newItem.CommendedNum = gradedData.CommendedNum;
                    
                    finalData.Add(newItem);
                }
                else
                {
                    numNull++;
                }
            }

            return finalData;
        }
    }
}