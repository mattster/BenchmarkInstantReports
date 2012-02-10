using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using System;

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
            //DataToGradeItemCollection scannedData = StudentData.GetStudentDataToGrade(dataservice, tests, schools);

            // go through each test
            foreach (Test curTest in tests)
            {
                // go through each school for this test
                foreach (School curSchool in schools)
                {
                    // get preslugged / queried data
                    PreslugData presluggedWithCurTestSch = StudentData.GetPreslugData(dataservice, curTest, curSchool);

                    // get scanned data
                    List<Test> singleTest = new List<Test>();
                    List<School> singleSchool = new List<School>();
                    singleTest.Add(curTest);
                    singleSchool.Add(curSchool);
                    DataToGradeItemCollection scannedWithCurTestSch = StudentData.GetStudentDataToGrade(dataservice, 
                        singleTest, singleSchool);

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
                    
            return finalData;
                    
                    
                    
                    
                    
            //        // add in all the preslugged / queried data
            //        PreslugData preslugged = StudentData.GetPreslugData(dataservice, curTest, curSchool);

            //        foreach (PreslugItem preslugitem in preslugged.GetItems())
            //        {
            //            ScanReportItem newItem = new ScanReportItem();
            //            newItem.Campus = preslugitem.Campus;
            //            newItem.TestID = curTest.TestID;
            //            newItem.Teacher = preslugitem.TeacherName;
            //            newItem.Period = preslugitem.Period;
            //            newItem.NumScanned = 0;
            //            newItem.NumQueried = 1;

            //            ScanReportItemKey findthiskey = new ScanReportItemKey(newItem);
            //            if (finalDataH.ContainsKey(findthiskey))
            //            {
            //                // this item is here
            //                ScanReportItem founditem = finalDataH[findthiskey] as ScanReportItem;
            //                founditem.NumScanned += newItem.NumScanned;
            //                founditem.NumQueried += newItem.NumQueried;
            //                finalDataH[findthiskey] = founditem;
            //            }
            //            else
            //            {
            //                // not here yet - add it
            //                finalDataH.Add(findthiskey, newItem);
            //            }
            //        }

            //        // add in all the scanned data
            //        foreach (DataToGradeItem scanneditem in scannedData.GetItemsWhere(d =>
            //                                                                          d.TestID == curTest.TestID &&
            //                                                                          d.Campus == curSchool.Abbr))
            //        {
            //            ScanReportItem newItem = new ScanReportItem();
            //            newItem.Campus = curSchool.Abbr;
            //            newItem.TestID = curTest.TestID;
            //            newItem.Teacher = scanneditem.TeacherName;
            //            newItem.Period = scanneditem.Period;
            //            newItem.NumScanned = 1;
            //            newItem.NumQueried = 0;

            //            ScanReportItemKey findthiskey = new ScanReportItemKey(newItem);
            //            if (finalDataH.ContainsKey(findthiskey))
            //            {
            //                // this item is here
            //                ScanReportItem founditem = finalDataH[findthiskey] as ScanReportItem;
            //                founditem.NumScanned += newItem.NumScanned;
            //                founditem.NumQueried += newItem.NumQueried;
            //                finalDataH[findthiskey] = founditem;
            //            }
            //            else
            //            {
            //                // not here yet - add it
            //                finalDataH.Add(findthiskey, newItem);
            //            }
            //        }
            //    }
            //}

            //return new ScanReportData(finalDataH.Values.Cast<ScanReportItem>().ToList());
        }
    }
}