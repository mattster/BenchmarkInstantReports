using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;

namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    public class ScanRepHelper
    {
        public static ScanReportData generateScanRepTable(string campus, string[] benchmarks)
        {
            //DataTable table = new DataTable();
            ScanReportData finalData = new ScanReportData();

            //// create columns for the results table
            //table.Columns.Add(new DataColumn(lblCampus, System.Type.GetType("System.String")));
            //table.Columns.Add(new DataColumn(lblTestId, System.Type.GetType("System.String")));
            //table.Columns.Add(new DataColumn(lblTeacher, System.Type.GetType("System.String")));
            //table.Columns.Add(new DataColumn(lblPeriod, System.Type.GetType("System.String")));
            //table.Columns.Add(new DataColumn(lblNumScanned, System.Type.GetType("System.Int32")));
            //table.Columns.Add(new DataColumn(lblNumQueried, System.Type.GetType("System.Int32")));


            // get data for number queried for the specified tests
            //DataSet dsQueried = birIF.executeStudentListQuery(benchmarks[0], campus);
            PreslugData preslugged = ScanHelper.ReturnPreslugData(benchmarks[0], campus);

            // go through the list of queried records
            //for (int k = 0; k < dsQueried.Tables[0].Rows.Count; k++)
            for (int k = 0; k < preslugged.Count; k++)
            {
                //DataRow newrow = table.NewRow();
                ScanReportItem newItem = new ScanReportItem();
                //newrow[lblCampus] = dsQueried.Tables[0].Rows[k]["SCHOOL2"].ToString();
                newItem.Campus = preslugged.Idx(k).Campus;
                //newrow[lblTestId] = benchmarks[0];
                newItem.TestID = benchmarks[0];
                //newrow[lblTeacher] = dsQueried.Tables[0].Rows[k][Constants.TeacherNameFieldName].ToString();
                newItem.Teacher = preslugged.Idx(k).TeacherName;
                //newrow[lblPeriod] = dsQueried.Tables[0].Rows[k][lblPeriod].ToString();
                newItem.Period = preslugged.Idx(k).Period;
                //newrow[lblNumScanned] = 0;                                  // we'll count this later
                newItem.NumScanned = 0;                                     // we'll cound this later
                //newrow[lblNumQueried] = 1;
                newItem.NumQueried = 1;
                //addRowToScanResultsTable(newrow, table);
                finalData.Add(newItem);
            }


            // if there is more than 1 benchmark, get the rest of the queried data
            if (benchmarks.Length > 1)
            {
                for (int i = 1; i < benchmarks.Length; i++)                 // we already did benchmarks[0]
                {
                    //DataSet tempds = birIF.executeStudentListQuery(benchmarks[i], campus);
                    PreslugData preslugged2 = ScanHelper.ReturnPreslugData(benchmarks[i], campus);

                    // go through the list of queried records
                    //for (int k = 0; k < tempds.Tables[0].Rows.Count; k++)
                    for (int k = 0; k < preslugged2.Count; k++)
                    {
                        //DataRow newrow = table.NewRow();
                        ScanReportItem newItem = new ScanReportItem();
                        newItem.Campus = preslugged.Idx(k).Campus;
                        newItem.TestID = benchmarks[0];
                        newItem.Teacher = preslugged.Idx(k).TeacherName;
                        newItem.Period = preslugged.Idx(k).Period;
                        newItem.NumScanned = 0;                                     // we'll cound this later
                        newItem.NumQueried = 1;
                        finalData.Add(newItem);
                    }
                }
            }

            // get student scanned data for the specified tests
            //DataSet dsStudentScans = birIF.getStudentScanListData(benchmarks[0], campus);                         /////////////////////////////////////////
            List<StudentListItem> studentScans = ScanHelper.GetStudentScanListData(benchmarks[0], campus);
            //DataSet dsStudentScans = new DataSet();
            //if (campus == "ALL Elementary" || campus == "ALL Secondary")
            //    dsStudentScans = birIF.getStudentDataToGrade(benchmarks[0]);
            //else
            //    dsStudentScans = birIF.getStudentDataToGrade(benchmarks[0], campus);

            // if there are more than 1 benchmark, get the rest of the student scan data
            if (benchmarks.Length > 1)
            {
                for (int ii = 1; ii < benchmarks.Length; ii++)              // we already did benchmarks[0]
                {
                    //DataSet tempds = birIF.getStudentScanListData(benchmarks[ii], campus);
                    List<StudentListItem> studentScans2 = ScanHelper.GetStudentScanListData(benchmarks[ii], campus);
                    //DataSet tempds = new DataSet();
                    //if (campus == "ALL Elementary" || campus == "ALL Secondary")
                    //    tempds = birIF.getStudentDataToGrade(benchmarks[0]);
                    //else
                    //    tempds = birIF.getStudentDataToGrade(benchmarks[0], campus);
                    //joinDS(dsStudentScans, tempds);
                    studentScans.AddRange(studentScans2);
                }
            }

            // go through the list of scanned records
            //for (int j = 0; j < dsStudentScans.Tables[0].Rows.Count; j++)
            for (int j = 0; j < studentScans.Count; j++)
            {
                //DataRow newrow = table.NewRow();
                ScanReportItem newItem = new ScanReportItem();
                //newrow[lblCampus] = dsStudentScans.Tables[0].Rows[j]["SCHOOL2"].ToString();
                newItem.Campus = studentScans[j].Campus;
                //newrow[lblTestId] = dsStudentScans.Tables[0].Rows[j][lblTestId].ToString();
                newItem.TestID = studentScans[j].TestID;
                //newrow[lblTeacher] = dsStudentScans.Tables[0].Rows[j][Constants.TeacherNameFieldName].ToString();
                newItem.Teacher = studentScans[j].TeacherName;
                //newrow[lblPeriod] = dsStudentScans.Tables[0].Rows[j][lblPeriod].ToString();
                newItem.Period = studentScans[j].Period;
                //newrow[lblNumScanned] = 1;
                newItem.NumScanned = 1;
                //newrow[lblNumQueried] = 0;                              // we already counted this above
                newItem.NumQueried = 0;                                 // we already counted this above
                //addRowToScanResultsTable(newrow, table);
                finalData.AddOrMerge(newItem);
            }

            return finalData;
        }
    }
}