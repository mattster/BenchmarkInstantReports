using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;

namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    public class ScanRepHelper
    {
        public static ScanReportData generateScanRepTable(string campus, string[] tests)
        {
            Hashtable finalDataH = new Hashtable();

            foreach (string test in tests)
            {
                // add in all the preslugged / queried data
                PreslugData preslugged = ScanHelper.ReturnPreslugData(test, campus);

                foreach (PreslugItem preslugitem in preslugged.GetItems())
                {
                    ScanReportItem newItem = new ScanReportItem();
                    newItem.Campus = preslugitem.Campus;
                    newItem.TestID = test;
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
                List<DataToGradeItem> studentScans = ScanHelper.GetStudentScanListData(test, campus);

                foreach (DataToGradeItem scanneditem in studentScans)
                {
                    ScanReportItem newItem = new ScanReportItem();
                    newItem.Campus = scanneditem.Campus;
                    newItem.TestID = test;
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

            return new ScanReportData(finalDataH.Values.Cast<ScanReportItem>().ToList());
        }
    }
}