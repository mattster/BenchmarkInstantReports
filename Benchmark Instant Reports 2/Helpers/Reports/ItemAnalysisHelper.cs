using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Interfaces;
using System.Data;

namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    public class ItemAnalysisHelper
    {
        public static List<ResultsTableItem> generateBenchmarkStatsRepTable(List<StudentScanDataItem> studentData, string curTest, string curCampus)
        {
            //DataView dvStudentData = new DataView(studentData);
            string curId;
            //DataSet dsFinal = new DataSet();
            List<ResultsTableItem> returnData = new List<ResultsTableItem>();
            List<ResultsTableItem> resultsData = new List<ResultsTableItem>();
            //DataTable resultsTable = new DataTable();
            int curNumCorrect, curNumTotal = new int();
            AnswerCounter curAnsCount = new AnswerCounter();

            //setupResultsDataTable(resultsTable);

            // grade each student's test and add it to the DataSet
            int numNull = 0;
            //for (int jj = 0; jj < dvStudentData.Table.Rows.Count; jj++)
            for (int jj = 0; jj < studentData.Count; jj++)
            {
                //curId = dvStudentData.Table.Rows[jj]["local_student_id"].ToString();
                curId = studentData[jj].StudentID;

                //DataRow curScanDataRow = birIF.getLatestScanDataRow(curId, curTest);
                ScanItem curScanDataItem = birIF.getLatestScanDataRowq(curId, curTest);
                //if (curScanDataRow != null)
                if (curScanDataItem != null)
                {
                    //DataTable gradedTable = birIF.gradeScannedTestDetail(curTest, curScanDataRow["ANSWERS"].ToString(), curCampus, 0,
                    //    dvStudentData.Table.Rows[jj][Constants.TeacherNameFieldName].ToString(),
                    //    dvStudentData.Table.Rows[jj]["PERIOD"].ToString());
                    DataTable gradedTable = birIF.gradeScannedTestDetail(curTest, curScanDataItem.Answers, curCampus, 0,
                        studentData[jj].TeacherName, studentData[jj].Period);

                    
                    // add data for each test item to the resultsTable
                    for (int k = 0; k < gradedTable.Rows.Count; k++)
                    {
                        // see if this item is already in the dataset
                        string selectString =
                            //"CAMPUS = \'" + dvStudentData.Table.Rows[j]["SCHOOL_ABBR"].ToString() + "\' and " +
                            "CAMPUS = \'" + dvStudentData.Table.Rows[jj]["SCHOOL2"].ToString() + "\' and " +
                            "TEST_ID = \'" + curTest + "\' and " +
                            "TEACHER = \'" + dvStudentData.Table.Rows[jj][Constants.TeacherNameFieldName].ToString().Replace("'", "''") + "\' and " +
                            "PERIOD = \'" + dvStudentData.Table.Rows[jj]["PERIOD"].ToString() + "\' and " +
                            "ITEM_NUM = " + gradedTable.Rows[k]["ITEM_NUM"];
                        DataRow[] selectedRows = resultsTable.Select(selectString);

                        if (selectedRows.Length > 0)
                        {
                            // this item is here - get the values and then delete it
                            curNumCorrect = (int)selectedRows[0][lblNumCorrect];
                            curNumTotal = (int)selectedRows[0][lblNumTotal];
                            curAnsCount = getAnswerCounts(selectedRows[0]);

                            resultsTable.Rows.Remove(selectedRows[0]);
                        }
                        else
                        {
                            curNumCorrect = 0;
                            curNumTotal = 0;
                            curAnsCount.Reset();
                        }

                        // add the row to the results table
                        DataRow thisrow = resultsTable.NewRow();
                        //thisrow[lblCampus] = dvStudentData.Table.Rows[j]["SCHOOL_ABBR"].ToString();
                        thisrow[lblCampus] = dvStudentData.Table.Rows[jj]["SCHOOL2"].ToString();
                        thisrow[lblTestId] = curTest;
                        thisrow[lblTeacher] = dvStudentData.Table.Rows[jj][Constants.TeacherNameFieldName].ToString();
                        thisrow[lblPeriod] = dvStudentData.Table.Rows[jj]["PERIOD"].ToString();
                        thisrow[lblItemNum] = gradedTable.Rows[k]["ITEM_NUM"];
                        if ((bool)gradedTable.Rows[k]["CORRECT"])
                            curNumCorrect++;
                        curAnsCount.Increment(gradedTable.Rows[k]["STUDENT_ANS"].ToString());
                        curNumTotal++;
                        thisrow[lblNumCorrect] = curNumCorrect;
                        thisrow[lblNumTotal] = curNumTotal;
                        thisrow[lblPctCorrect] = (decimal)curNumCorrect / (decimal)curNumTotal;
                        putAnswerCounts(curAnsCount, thisrow);
                        thisrow[lblAnswer] = gradedTable.Rows[k]["CORRECT_ANS"].ToString();
                        thisrow[lblObjective] = gradedTable.Rows[k]["OBJECTIVE"];
                        thisrow[lblTEKS] = gradedTable.Rows[k]["TEKS"];

                        resultsTable.Rows.Add(thisrow);
                    }

                }
                else
                {
                    numNull++;
                }
            }

            return resultsTable;
        }


    }
}