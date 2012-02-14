using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Infrastructure.DataStruct;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;


namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    public class StGradesRepHelper
    {
        /// <summary>
        /// Generate data for the Student Grades Report
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="studentData">student data to grade</param>
        /// <param name="tests">list of Test objects that are included in studentData</param>
        /// <returns>StGradeReportData object containing data ready to display in the report</returns>
        public static StGradeReportData GenerateStudentGradesReportData(IRepoService dataservice,
            DataToGradeItemCollection studentData, List<Test> tests)
        {
            StGradeReportData finalData = new StGradeReportData();

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
                            var curDataToGrade = dataWithCurTestSchTch.Where(d => d.Period == curPeriod);
                            AnswerKeyItemData curAnsKey = AnswerKeyHelper.GetTestAnswerKeyData(dataservice, curTest,
                                curSchoolAbbr, curTeacher, curPeriod);

                            int numNull = 0;
                            foreach (DataToGradeItem itemToGrade in curDataToGrade)
                            {
                                if (itemToGrade.ScanItem.AnswerString != null)
                                {

                                    GradedTestData gradedData = GradeTests.GradeStudentAnswers(curTest, 
                                        itemToGrade.ScanItem.AnswerString, curAnsKey);

                                    StGradeReportItem newItem = new StGradeReportItem();
                                    newItem.StudentID = itemToGrade.StudentID;
                                    newItem.StudentName = itemToGrade.StudentName;
                                    newItem.TestID = itemToGrade.TestID;
                                    newItem.ScanDate = itemToGrade.ScanItem.DateScanned;
                                    newItem.Campus = itemToGrade.Campus;
                                    newItem.Teacher = itemToGrade.TeacherName;
                                    newItem.Period = itemToGrade.Period;

                                    newItem.LetterGrade = gradedData.LetterGrade;
                                    newItem.NumCorrect = gradedData.NumCorrect;
                                    newItem.NumTotal = gradedData.NumTotal;
                                    newItem.NumPoints = gradedData.NumPoints;
                                    newItem.NumTotalPoints = gradedData.NumTotalPoints;
                                    newItem.PctCorrect = gradedData.PctCorrect;
                                    newItem.PassNum = gradedData.PassNum;
                                    newItem.CommendedNum = gradedData.CommendedNum;
                                    newItem.GradedAnswers = gradedData.GradedAnswers;
                                    newItem.GradedAnswersFormatted = gradedData.GradedAnswersFormatted;

                                    finalData.Add(newItem);
                                }
                                else
                                {
                                    numNull++;
                                }
                            }
                        }
                    }
                }
            }

            return finalData;
        }
    }
}