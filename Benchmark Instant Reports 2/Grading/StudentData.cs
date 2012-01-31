using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using System.Data;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Helpers.Reports;
using Benchmark_Instant_Reports_2.Helpers;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;

namespace Benchmark_Instant_Reports_2.Grading
{
    public class StudentData
    {
        public static List<StudentListItem> GetStudentDataToGrade(IRepoService dataservice, string testID, string campus, 
            string teacher = "",
            string periodList = "'00','01','02','03','04','05','06','07','08','09','10','11','12','13','14'")
        {
            List<string> studIdList1 = new List<string>();

            //// get a set of the student scans for this test and campus
            
            // get a set of students who meet the criteria for this test


            PreslugData preslugged = ScanHelper.ReturnPreslugData(dataservice, testID, campus);
            string[] teacherList = preslugged.GetItems().Select(p => p.TeacherName).Distinct().ToArray();
            Array.Sort(teacherList);


            // get student data for students who have scans and meet the test criteria
            List<StudentListItem> PresluggedStudentsWithScans = new List<StudentListItem>();
            if (teacher == "")
                PresluggedStudentsWithScans = ScanHelper.GetStudentScanListData(testID, campus);
            else
                PresluggedStudentsWithScans = ScanHelper.GetStudentScanListData(testID, campus, teacher, periodList);
            List<StudentListItem> returnData = PresluggedStudentsWithScans;

            // get student ID's for students who have scans but do not meet the test criteria
            List<string> studentIDsWithScansNotInTestCriteria = DBIOWorkaround.ReturnStudentIDsWScansNotPreslugged(testID, campus);

            // for any students who do not match test criteria, try to match them up somehow
            if (studentIDsWithScansNotInTestCriteria.Count > 0)
            {
                foreach (string curstudentid in studentIDsWithScansNotInTestCriteria)
                {
                    string studentId = curstudentid;
                    if (studentId.Length == 5)              // add a leading 0 if necessary - makes things better
                        studentId = "0" + studentId;

                    if (birUtilities.isTeacherUnknown(studentId))
                    {
                        StudentListItem newItem = new StudentListItem();
                        newItem.StudentID = studentId;
                        newItem.StudentName = "";
                        newItem.TeacherName = Constants.UnknownTeacherName;
                        newItem.Period = "01";
                        newItem.CourseID = "00000";
                        newItem.Campus = campus;
                        newItem.TestID = testID;
                        returnData.Add(newItem);
                    }
                    else
                    {

                        // try to match them to teachers in the test query
                        string qs = Queries.MatchStudentToTeacherList.Replace("@testId", testID);
                        qs = qs.Replace("@studentId", studentId);
                        qs = qs.Replace("@teacherList", birUtilities.convertStringArrayForQuery(teacherList));
                        List<StudentListItem> StudentMatches = DBIOWorkaround.ReturnStudentScanDataItemsFromQ(qs);
                        if (StudentMatches.Count > 0)
                        {
                            StudentListItem newItem = new StudentListItem();
                            newItem = StudentMatches[0];
                            returnData.Add(newItem);
                        }

                        else
                        {
                            // try to match them up to teachers when removing the BENCHMARK_MOD criteria
                            string customQuery = DBIOWorkaround.ReturnRawCustomQuery(testID);
                            string customQueryNoMod = customQuery.Replace("AND BENCHMARK_MOD LIKE \'____1\'", " ");
                            customQueryNoMod = customQueryNoMod.Replace("AND BENCHMARK_MOD LIKE \'___1_\'", " ");
                            customQueryNoMod = customQueryNoMod.Replace("AND BENCHMARK_MOD LIKE \'__1__\'", " ");
                            customQueryNoMod = customQueryNoMod.Replace("AND BENCHMARK_MOD LIKE \'_1___\'", " ");
                            customQueryNoMod = customQueryNoMod.Replace("AND BENCHMARK_MOD LIKE \'1____\'", " ");
                            List<StudentListItem> StudentMatches2 = DBIOWorkaround.ReturnStudentScanDataItemsFromQ(customQueryNoMod);
                            if (StudentMatches2.Count > 0)
                            {
                                string selectStudentFilter = "LOCAL_STUDENT_ID = \'" + studentId + "\'";
                                List<StudentListItem> matchingData =
                                    StudentMatches2.FindAll(delegate(StudentListItem item)
                                    {
                                        return item.StudentID == studentId;
                                    });
                                int c = matchingData.Count();
                                if (matchingData.Count() > 0)
                                {
                                    StudentListItem newItem = new StudentListItem();
                                    newItem.StudentID = matchingData[0].StudentID;
                                    newItem.StudentName = matchingData[0].StudentName;
                                    newItem.TeacherName = matchingData[0].TeacherName;
                                    newItem.Period = matchingData[0].Period;
                                    newItem.CourseID = matchingData[0].CourseID;
                                    newItem.Campus = matchingData[0].Campus;
                                    newItem.TestID = testID;
                                    returnData.Add(newItem);
                                }

                                else
                                {
                                    // no matching teacher found in query removing BENCHMARK_MOD criteria, put "UNKNOWN"
                                    StudentListItem newItem = new StudentListItem();
                                    newItem.StudentID = studentId;
                                    newItem.TeacherName = Constants.UnknownTeacherName;
                                    newItem.Period = "01";
                                    newItem.CourseID = "00000";
                                    newItem.Campus = campus;
                                    newItem.TestID = testID;
                                    returnData.Add(newItem);
                                }
                            }

                            else
                            {
                                // no rows found when removing BENCHMARK_MOD criteria, put "UNKNOWN"
                                StudentListItem newItem = new StudentListItem();
                                newItem.StudentID = studentId;
                                newItem.TeacherName = Constants.UnknownTeacherName;
                                newItem.Period = "01";
                                newItem.CourseID = "00000";
                                newItem.Campus = campus;
                                newItem.TestID = testID;
                                returnData.Add(newItem);
                            }
                        }
                    }
                }
            }

            return returnData;
        }

    
    
    }
}