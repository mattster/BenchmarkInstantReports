using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;
using Benchmark_Instant_Reports_2.Infrastructure.IRepositories;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;

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
            DataToGradeItemCollection scannedData = StudentData.GetStudentDataToGrade(dataservice, tests, schools);

            foreach (School curSchool in schools)
            {
                foreach (Test curTest in tests)
                {
                    // add in all the preslugged / queried data
                    PreslugData preslugged = StudentData.GetPreslugData(dataservice, curTest, curSchool);

                    foreach (PreslugItem preslugitem in preslugged.GetItems())
                    {
                        ScanReportItem newItem = new ScanReportItem();
                        newItem.Campus = preslugitem.Campus;
                        newItem.TestID = curTest.TestID;
                        newItem.Teacher = preslugitem.TeacherName;
                        newItem.Period = preslugitem.Period;
                        newItem.NumScanned = 0;
                        newItem.NumQueried = 1;

                        ScanReportItemKey findthiskey = new ScanReportItemKey(newItem);
                        if (finalDataH.ContainsKey(findthiskey))
                        {
                            // this item is here
                            ScanReportItem founditem = finalDataH[findthiskey] as ScanReportItem;
                            founditem.NumScanned += newItem.NumScanned;
                            founditem.NumQueried += newItem.NumQueried;
                            finalDataH[findthiskey] = founditem;
                        }
                        else
                        {
                            // not here yet - add it
                            finalDataH.Add(findthiskey, newItem);
                        }
                    }

                    // add in all the scanned data
                    foreach (DataToGradeItem scanneditem in scannedData.GetItemsWhere(d => 
                                                                                      d.TestID == curTest.TestID && 
                                                                                      d.Campus == curSchool.Abbr))
                    {
                        ScanReportItem newItem = new ScanReportItem();
                        newItem.Campus = curSchool.Abbr;
                        newItem.TestID = curTest.TestID;
                        newItem.Teacher = scanneditem.TeacherName;
                        newItem.Period = scanneditem.Period;
                        newItem.NumScanned = 1;
                        newItem.NumQueried = 0;

                        ScanReportItemKey findthiskey = new ScanReportItemKey(newItem);
                        if (finalDataH.ContainsKey(findthiskey))
                        {
                            // this item is here
                            ScanReportItem founditem = finalDataH[findthiskey] as ScanReportItem;
                            founditem.NumScanned += newItem.NumScanned;
                            founditem.NumQueried += newItem.NumQueried;
                            finalDataH[findthiskey] = founditem;
                        }
                        else
                        {
                            // not here yet - add it
                            finalDataH.Add(findthiskey, newItem);
                        }
                    }
                }
            }

            return new ScanReportData(finalDataH.Values.Cast<ScanReportItem>().ToList());
        }
    }
}