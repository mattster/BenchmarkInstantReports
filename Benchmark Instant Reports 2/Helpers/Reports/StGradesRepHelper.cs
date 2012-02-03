using Benchmark_Instant_Reports_2.Exceptions;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using System.Linq;


namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    public class StGradesRepHelper
    {
        public static StGradeReportData GenerateStudentGradesReportData(DataToGradeItemCollection studentData, IQueryable<Test> tests)
        {
            string curId;
            StGradeReportData finalData = new StGradeReportData();
            int ansKeyVersionIncrement = new int();

            // grade each student's test and add it to the DataSet
            int numNull = 0;
            foreach (DataToGradeItem item in studentData.GetItems())
            {
                curId = item.StudentID;
                string curCampus = item.Campus;
                
                if (item.ScanItem.AnswerString != null)
                {
                    ansKeyVersionIncrement = ExceptionHandler.campusAnswerKeyVersionIncrement(item.TestID, curCampus,
                        item.TeacherName, item.Period);

                    GradedTestData gradedData = GradeTests.GradeTest(test, item.ScanItem.AnswerString, curCampus, ansKeyVersionIncrement,
                        item.TeacherName, item.Period);

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