﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Infrastructure.DataStruct;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;

namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    public class ScanRepHelper
    {
        /// <summary>
        /// Generate the data for the Scan Report
        /// </summary>
        /// <param name="dataservice">IRepository access to the data</param>
        /// <param name="schools">List of School objects to use</param>
        /// <param name="tests">List of Test objects to use</param>
        /// <returns>ScanReportData object with the data for the Scan Report</returns>
        public static ScanReportData GenerateScanReportData(IRepoService dataservice, List<School> schools, List<Test> tests)
        {
            Hashtable finalDataH = new Hashtable();
            ScanReportData finalData = new ScanReportData();

            // go through each test
            foreach (Test curTest in tests)
            {
                IQueryable<Scan> scannedWithCurTest = StudentData.GetScannedData(dataservice, curTest);

                // go through each school for this test
                foreach (School curSchool in schools)
                {
                    // get preslugged / queried data
                    PreslugData presluggedWithCurTestSch = StudentData.GetPreslugData(dataservice, curTest, curSchool);

                    if (presluggedWithCurTestSch.Count > 0)
                    {
                        // get scanned data
                        DataToGradeItemCollection scannedWithCurTestSch = StudentData.GetStudentDataToGrade(dataservice,
                            curTest, curSchool, presluggedWithCurTestSch, scannedWithCurTest);

                        // go through each teacher for this test and school
                        var teachers = presluggedWithCurTestSch.GetItems().Select(ps => ps.TeacherName).Distinct();
                        teachers = teachers.Union(scannedWithCurTestSch.GetItems().Select(sd => sd.TeacherName).Distinct());
                        foreach (string curTeacher in teachers)
                        {
                            // go through each period for this test and school and teacher
                            var presluggedWithCurTestSchTch = presluggedWithCurTestSch.
                                GetItemsWhere(ps => ps.TeacherName == curTeacher);
                            var scannedWithCurTestSchTch = scannedWithCurTestSch.
                                GetItemsWhere(sd => sd.TeacherName == curTeacher);
                            var periods = presluggedWithCurTestSchTch.Select(ps => ps.Period).Distinct();
                            periods = periods.Union(scannedWithCurTestSchTch.Select(sd => sd.Period).Distinct());
                            foreach (string curPeriod in periods)
                            {
                                ScanReportItem newItem = new ScanReportItem();
                                newItem.Campus = curSchool.Abbr;
                                newItem.TestID = curTest.TestID;
                                newItem.Teacher = curTeacher;
                                newItem.Period = curPeriod;
                                newItem.NumScanned = scannedWithCurTestSchTch.Where(sd => sd.Period == curPeriod).Count();
                                newItem.NumQueried = presluggedWithCurTestSchTch.Where(ps => ps.Period == curPeriod).Count();
                                finalData.Add(newItem);
                            }
                        }
                    }
                }
            }

            return finalData;
        }


        /// <summary>
        /// find a list of students who meet the criteria for a test but have no scans for that test
        /// </summary>
        /// <param name="dataservice">IRepoService access to data</param>
        /// <param name="schools">list of applicable schools to check</param>
        /// <param name="tests">list of applicable tests to check</param>
        /// <returns>student data via a PreslugData item collection</returns>
        public static PreslugData GenerateMissingStudentData(IRepoService dataservice, List<School> schools, List<Test> tests)
        {
            List<PreslugData> finalDataList = new List<PreslugData>();
            PreslugData finalData = new PreslugData();

            // go through each test
            foreach (Test curTest in tests)
            {
                IQueryable<Scan> scannedWithCurTest = StudentData.GetScannedData(dataservice, curTest);

                // go through each school for this test
                foreach (School curSchool in schools)
                {
                    // get preslugged / queried data
                    PreslugData presluggedWithCurTestSch = StudentData.GetPreslugData(dataservice, curTest, curSchool);

                    if (presluggedWithCurTestSch.Count > 0)
                    {
                        // get scanned data
                        DataToGradeItemCollection scannedWithCurTestSch = StudentData.GetStudentDataToGrade(dataservice,
                            curTest, curSchool, presluggedWithCurTestSch, scannedWithCurTest);

                        // hey this is kinda cool
                        PreslugData presluggedNotScanned = StudentData.GetPresluggedNotScanned(presluggedWithCurTestSch,
                            scannedWithCurTestSch);
                        finalDataList.Add(presluggedNotScanned);
                    }
                }
            }

            foreach (var preslugdata in finalDataList)
            {
                foreach (var preslugitem in preslugdata.GetItems())
                {
                    if (finalData.GetItemsWhere(d => d.StudentID == preslugitem.StudentID).Count() == 0)
                        finalData.Add(preslugitem);
                }
            }

            return finalData;
        }
    }
}