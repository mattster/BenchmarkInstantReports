using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Interfaces.DBDataStruct;
using Benchmark_Instant_Reports_2.Interfaces;
using System.Data;
using Benchmark_Instant_Reports_2.Grading;
using Benchmark_Instant_Reports_2.References;
using System.Configuration;
using System.Data.OracleClient;

namespace Benchmark_Instant_Reports_2.Helpers.Reports
{
    public class ItemAnalysisHelper
    {
        public static List<ResultsTableItem> generateBenchmarkStatsRepTableQ(List<StudentScanDataItem> studentData, string curTest, string curCampus)
        {
            //DataView dvStudentData = new DataView(studentData);
            string curId;
            //DataSet dsFinal = new DataSet();
            List<ResultsTableItem> finalData = new List<ResultsTableItem>();
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
                    List<GradedItemDetail> gradedData = GradeTest.gradeScannedTestDetailQ(curTest, curScanDataItem.Answers, curCampus, 0,
                        studentData[jj].TeacherName, studentData[jj].Period);

                    
                    // add data for each test item to the resultsTable
                    //for (int k = 0; k < gradedTable.Rows.Count; k++)
                    for (int k = 0; k < gradedData.Count; k++)
                    {
                        // see if this item is already in the dataset
                        //string selectString =
                        //    //"CAMPUS = \'" + dvStudentData.Table.Rows[j]["SCHOOL_ABBR"].ToString() + "\' and " +
                        //    "CAMPUS = \'" + dvStudentData.Table.Rows[jj]["SCHOOL2"].ToString() + "\' and " +
                        //    "TEST_ID = \'" + curTest + "\' and " +
                        //    "TEACHER = \'" + dvStudentData.Table.Rows[jj][Constants.TeacherNameFieldName].ToString().Replace("'", "''") + "\' and " +
                        //    "PERIOD = \'" + dvStudentData.Table.Rows[jj]["PERIOD"].ToString() + "\' and " +
                        //    "ITEM_NUM = " + gradedTable.Rows[k]["ITEM_NUM"];
                        //DataRow[] selectedRows = resultsTable.Select(selectString);

                        List<ResultsTableItem> foundItems = finalData.FindAll(delegate(ResultsTableItem rti)
                        {
                            if (rti.Campus == studentData[jj].Campus &&
                                rti.TestID == curTest &&
                                rti.Teacher == studentData[jj].TeacherName &&
                                rti.Period == studentData[jj].Period &&
                                rti.ItemNum == gradedData[k].ItemNum)
                                return true;

                            return false;
                        });

                        //if (selectedRows.Length > 0)
                        if (foundItems.Count > 0)
                        {
                            // this item is here - get the values and then delete it
                            //curNumCorrect = (int)selectedRows[0][lblNumCorrect];
                            curNumCorrect = foundItems[0].NumCorrect;
                            //curNumTotal = (int)selectedRows[0][lblNumTotal];
                            curNumTotal = foundItems[0].NumTotal;
                            //curAnsCount = getAnswerCounts(selectedRows[0]);
                            curAnsCount.UpdateFromResultsTableItem(foundItems[0]);

                            //resultsTable.Rows.Remove(selectedRows[0]);
                            finalData.Remove(finalData.Find(delegate(ResultsTableItem rti)
                            { 
                                if (rti.TestID == foundItems[0].TestID &&
                                    rti.Campus == foundItems[0].Campus &&
                                    rti.Teacher == foundItems[0].Teacher &&
                                    rti.Period == foundItems[0].Period &&
                                    rti.ItemNum == foundItems[0].ItemNum)
                                    return true;

                                return false;
                            }));
                            
                        }
                        else
                        {
                            curNumCorrect = 0;
                            curNumTotal = 0;
                            curAnsCount.Reset();
                        }

                        // add the row to the results table
                        //DataRow thisrow = resultsTable.NewRow();
                        ResultsTableItem newItem = new ResultsTableItem();
                        //thisrow[lblCampus] = dvStudentData.Table.Rows[j]["SCHOOL_ABBR"].ToString();
                        //thisrow[lblCampus] = dvStudentData.Table.Rows[jj]["SCHOOL2"].ToString();
                        newItem.Campus = studentData[jj].Campus;
                        //thisrow[lblTestId] = curTest;
                        newItem.TestID = curTest;
                        //thisrow[lblTeacher] = dvStudentData.Table.Rows[jj][Constants.TeacherNameFieldName].ToString();
                        newItem.Teacher = studentData[jj].TeacherName;
                        //thisrow[lblPeriod] = dvStudentData.Table.Rows[jj]["PERIOD"].ToString();
                        newItem.Period = studentData[jj].Period;
                        //thisrow[lblItemNum] = gradedTable.Rows[k]["ITEM_NUM"];
                        newItem.ItemNum = gradedData[k].ItemNum;
                        //if ((bool)gradedTable.Rows[k]["CORRECT"])
                        if (gradedData[k].Correct)
                            curNumCorrect++;
                        //curAnsCount.Increment(gradedTable.Rows[k]["STUDENT_ANS"].ToString());
                        curAnsCount.Increment(gradedData[k].StudentAnswer);
                        curNumTotal++;
                        //thisrow[lblNumCorrect] = curNumCorrect;
                        newItem.NumCorrect = curNumCorrect;
                        //thisrow[lblNumTotal] = curNumTotal;
                        newItem.NumTotal = curNumTotal;
                        //thisrow[lblPctCorrect] = (decimal)curNumCorrect / (decimal)curNumTotal;
                        newItem.PctCorrect = (decimal)curNumCorrect / (decimal)curNumTotal;
                        //putAnswerCounts(curAnsCount, thisrow);
                        curAnsCount.UpdateToResultsTableItem(newItem);
                        //thisrow[lblAnswer] = gradedTable.Rows[k]["CORRECT_ANS"].ToString();
                        newItem.Answer = gradedData[k].CorrectAnswer;
                        //thisrow[lblObjective] = gradedTable.Rows[k]["OBJECTIVE"];
                        newItem.Objective = gradedData[k].Category;
                        //thisrow[lblTEKS] = gradedTable.Rows[k]["TEKS"];
                        newItem.TEKS = gradedData[k].TEKS;

                        //resultsTable.Rows.Add(thisrow);
                        finalData.Add(newItem);
                    }

                }
                else
                {
                    numNull++;
                }
            }

            //return resultsTable;
            return finalData;
        }


        public static int writeBenchmarkStatsResultsToDbQ(List<ResultsTableItem> resultsData)
        {

            DataTable resultsTable = convertRTIListToTable(resultsData);


            int result = new int();
            DataSet ds = new DataSet();
            string qs = "SELECT * FROM " + DatabaseDefn.DBResultsBenchmarkStats;
            string findcriteria1 = "";

            // get connection to the database
            string connectionString = ConfigurationManager.ConnectionStrings[dbIFOracle.databaseName].ConnectionString;
            using (OracleConnection connection =
                new OracleConnection(connectionString))
            {
                OracleDataAdapter adapter = new OracleDataAdapter();
                adapter.SelectCommand = new OracleCommand(qs, connection);
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                OracleCommandBuilder cmdb = new OracleCommandBuilder(adapter);
                adapter.Fill(ds, DatabaseDefn.DBResultsBenchmarkStats);

                // write the data to the DataSet
                foreach (DataRow row in resultsTable.Rows)
                {
                    // check and see if current record already exists and is the latest one
                    findcriteria1 = "CAMPUS = \'" + row["CAMPUS"].ToString() + "\' " +
                                    "AND TEST_ID = \'" + row["TEST_ID"].ToString() + "\' " +
                                    "AND TEACHER = \'" + row["TEACHER"].ToString().Replace("'", "''") + "\' " +
                                    "AND PERIOD = \'" + row["PERIOD"].ToString() + "\' " +
                                    "AND ITEM_NUM = " + row["ITEM_NUM"].ToString();


                    int l = ds.Tables[0].Select(findcriteria1).Length;
                    if (l == 0)                 // row not there, add it to the dataset
                    {
                        DataRow row2 = ds.Tables[0].NewRow();
                        row2.ItemArray = row.ItemArray;
                        ds.Tables[0].Rows.Add(row2);
                    }
                    else                        // row is there, overwrite it if it's different data 
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if ((ds.Tables[0].Rows[i]["CAMPUS"].ToString() == row["CAMPUS"].ToString())
                                && (ds.Tables[0].Rows[i]["TEST_ID"].ToString() == row["TEST_ID"].ToString())
                                && (ds.Tables[0].Rows[i]["TEACHER"].ToString() == row["TEACHER"].ToString())
                                && (ds.Tables[0].Rows[i]["PERIOD"].ToString() == row["PERIOD"].ToString())
                                && (ds.Tables[0].Rows[i]["ITEM_NUM"].ToString() == row["ITEM_NUM"].ToString()))
                            {
                                if (ds.Tables[0].Rows[i].ItemArray != row.ItemArray)
                                {
                                    // overwrite the existing row with this one
                                    ds.Tables[0].Rows[i].ItemArray = row.ItemArray;
                                }
                            }
                            // otherwise just keep the existing one and move on
                        }
                    }
                }
                adapter.Update(ds, DatabaseDefn.DBResultsBenchmarkStats);
            }

            result = 1;

            return result;
        }





        private static DataTable convertRTIListToTable(List<ResultsTableItem> resultsData)
        {
            string lblCampus = "CAMPUS";
            string lblTestId = "TEST_ID";
            string lblTeacher = "TEACHER";
            string lblPeriod = "PERIOD";
            string lblItemNum = "ITEM_NUM";
            string lblPctCorrect = "PCT_CORRECT";
            string lblNumCorrect = "NUM_CORRECT";
            string lblNumTotal = "NUM_TOTAL";
            string lblNumA = "NUM_A";
            string lblNumB = "NUM_B";
            string lblNumC = "NUM_C";
            string lblNumD = "NUM_D";
            string lblNumE = "NUM_E";
            string lblNumF = "NUM_F";
            string lblNumG = "NUM_G";
            string lblNumH = "NUM_H";
            string lblNumJ = "NUM_J";
            string lblNumK = "NUM_K";
            string lblAnswer = "ANSWER";
            string lblObjective = "OBJECTIVE";
            string lblTEKS = "TEKS";

            // create columns for the resulting table
            DataTable rtable = new DataTable();
            rtable.Columns.Add(new DataColumn(lblCampus, System.Type.GetType("System.String")));
            rtable.Columns.Add(new DataColumn(lblTestId, System.Type.GetType("System.String")));
            rtable.Columns.Add(new DataColumn(lblTeacher, System.Type.GetType("System.String")));
            rtable.Columns.Add(new DataColumn(lblPeriod, System.Type.GetType("System.String")));
            rtable.Columns.Add(new DataColumn(lblItemNum, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblPctCorrect, System.Type.GetType("System.Decimal")));
            rtable.Columns.Add(new DataColumn(lblNumCorrect, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumTotal, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumA, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumB, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumC, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumD, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumE, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumF, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumG, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumH, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumJ, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblNumK, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblAnswer, System.Type.GetType("System.String")));
            rtable.Columns.Add(new DataColumn(lblObjective, System.Type.GetType("System.Int32")));
            rtable.Columns.Add(new DataColumn(lblTEKS, System.Type.GetType("System.String")));

            foreach (ResultsTableItem item in resultsData)
            {
                DataRow newRow = rtable.NewRow();
                newRow[lblCampus] = item.Campus;
                newRow[lblTestId] = item.TestID; 
                newRow[lblTeacher] = item.Teacher; 
                newRow[lblPeriod] = item.Period;
                newRow[lblItemNum] = item.ItemNum;
                newRow[lblPctCorrect] = item.PctCorrect;
                newRow[lblNumCorrect] = item.NumCorrect;
                newRow[lblNumTotal] = item.NumTotal;
                newRow[lblNumA] = item.NumA;
                newRow[lblNumB] = item.NumB;
                newRow[lblNumC] = item.NumC;
                newRow[lblNumD] = item.NumD;
                newRow[lblNumE] = item.NumE;
                newRow[lblNumF] = item.NumF;
                newRow[lblNumG] = item.NumG;
                newRow[lblNumH] = item.NumH;
                newRow[lblNumJ] = item.NumJ;
                newRow[lblNumK] = item.NumK;
                newRow[lblAnswer] = item.Answer;
                newRow[lblObjective] = item.Objective;
                newRow[lblTEKS] = item.TEKS;

                rtable.Rows.Add(newRow);
            }

            return rtable;
        }





    }
}