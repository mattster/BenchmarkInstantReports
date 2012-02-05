using Benchmark_Instant_Reports_2.Exceptions;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using System.Linq;
using System.Collections.Generic;


namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    public class StGradesRepHelper
    {
        /// <summary>
        /// Generate data for the Student Grades Report
        /// </summary>
        /// <param name="studentData">student data to grade</param>
        /// <param name="tests">list of Test objects that are included in studentData</param>
        /// <returns>DataToGradeItemCollection containing data ready to display in the report</returns>
        public static StGradeReportData GenerateStudentGradesReportData(DataToGradeItemCollection studentData, List<Test> tests)
        {
            StGradeReportData finalData = new StGradeReportData();
            int ansKeyVersionIncrement = new int();

            // grade each student's test and add it to the DataSet
            int numNull = 0;
            foreach (DataToGradeItem item in studentData.GetItems())
            {
                Test curTest = tests.Find(t => t.TestID == item.TestID);
                string curCampusAbbr = item.Campus;
                
                if (item.ScanItem.AnswerString != null)
                {
                    ansKeyVersionIncrement = ExceptionHandler.campusAnswerKeyVersionIncrement(curTest.TestID, curCampusAbbr,
                        item.TeacherName, item.Period);

                    GradedTestData gradedData = GradeTests.GradeTest(curTest, item.ScanItem.AnswerString, curCampusAbbr, 
                        ansKeyVersionIncrement, item.TeacherName, item.Period);

                    StGradeReportItem newItem = new StGradeReportItem();
                    newItem.StudentID = item.StudentID;
                    newItem.StudentName = item.StudentName;
                    newItem.TestID = item.TestID;
                    newItem.ScanDate = item.ScanItem.DateScanned;
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