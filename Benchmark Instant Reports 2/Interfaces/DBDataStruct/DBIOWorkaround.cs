using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Benchmark_Instant_Reports_2.References;
using Benchmark_Instant_Reports_2.Helpers;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    public class DBIOWorkaround
    {
        public static List<StudentListItem> ReturnStudentScanDataItemsFromQ(string qs)
        {
            DataSet ds = birIF.makeUniqueDataSet(dbIFOracle.getDataRows(qs));
            List<StudentListItem> returnList = new List<StudentListItem>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                StudentListItem newitem = new StudentListItem();
                newitem.StudentID = row["LOCAL_STUDENT_ID"].ToString();
                newitem.StudentName = row["STUDENT_NAME"].ToString();
                newitem.TeacherName = row[Constants.TeacherNameFieldName].ToString();
                newitem.Period = row["PERIOD"].ToString();
                newitem.CourseID = row["LOCAL_COURSE_ID"].ToString();
                newitem.Campus = row["SCHOOL2"].ToString();
                if (row.Table.Columns.Contains("TEST_ID"))
                    newitem.TestID = row["TEST_ID"].ToString();

                returnList.Add(newitem);
            }

            return returnList;
        }

        public static ScanItem ReturnScanItemFromQ(string qs)
        {
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];
            ScanItem item = new ScanItem();

            item.DateScannedStr = row["DATE_SCANNED"].ToString();
            item.ScanSequence = (int)(decimal)row["SCANNED_SEQUENCE"];
            item.Imagepath = row["IMAGEPATH"].ToString();
            item.Name = row["NAME"].ToString();
            item.StudentID = row["STUDENT_ID"].ToString();
            item.TestID = row["TEST_ID"].ToString();
            item.Language = row["LANGUAGE_VERSION"].ToString();
            item.Exempt = row["EXEMPT"].ToString();
            item.PreSlugged = row["PRESLUGGED"].ToString();
            item.Answers = row["ANSWERS"].ToString();

            return item;
        }

        public static ScanItemData ReturnDataFromScans(string qs)
        {
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            ScanItemData finalData = new ScanItemData();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ScanItem item = new ScanItem();

                item.DateScannedStr = row["DATE_SCANNED"].ToString();
                item.ScanSequence = (int)(decimal)row["SCANNED_SEQUENCE"];
                item.Imagepath = row["IMAGEPATH"].ToString();
                item.Name = row["NAME"].ToString();
                item.StudentID = row["STUDENT_ID"].ToString();
                item.TestID = row["TEST_ID"].ToString();
                item.Language = row["LANGUAGE_VERSION"].ToString();
                item.Exempt = row["EXEMPT"].ToString();
                item.PreSlugged = row["PRESLUGGED"].ToString();
                item.Answers = row["ANSWERS"].ToString();

                finalData.Add(item);
            }

            return finalData;
        }

        public static List<AnswerKeyItem> ReturnAnswerKey(string qs)
        {
            DataSet ds = dbIFOracle.getDataRows(qs);
            if (ds.Tables.Count == 0)
                return null;

            List<AnswerKeyItem> finalData = new List<AnswerKeyItem>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                AnswerKeyItem newItem = new AnswerKeyItem();
                newItem.TestID = row["TEST_ID"].ToString();
                newItem.ItemNum = (int)(decimal)row["ITEM_NUM"];
                newItem.Answer = row["ANSWER"].ToString();
                newItem.Category = (int)(decimal)row["OBJECTIVE"];
                newItem.TEKS = row["TEKS"].ToString();
                newItem.Weight = (decimal)row["WEIGHT"];

                finalData.Add(newItem);
            }

            return finalData;
        }
        
        public static string[] ReturnElemAbbrList()
        {
            string qs = Queries.GetELCampuses;
            DataSet ds = dbIFOracle.getDataRows(qs);

            List<string> abbrList = new List<string>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                abbrList.Add(row[0].ToString());
            }

            return abbrList.ToArray<string>();
        }

        public static string[] ReturnSecAbbrList()
        {
            string qs1 = Queries.GetJHCampuses;
            string qs2 = Queries.GetHSCampuses;
            DataSet ds1 = dbIFOracle.getDataRows(qs1);
            DataSet ds2 = dbIFOracle.getDataRows(qs2);

            ds1.Merge(ds2);

            List<string> abbrList = new List<string>();

            foreach (DataRow row in ds1.Tables[0].Rows)
            {
                abbrList.Add(row[0].ToString());
            }

            return abbrList.ToArray<string>();
        }

        public static PreslugData ReturnExecutedCustomQuery(string qs)
        {
            PreslugData finalData = new PreslugData();
            DataSet workingds = dbIFOracle.getDataRows(qs);

            foreach (DataRow row in workingds.Tables[0].Rows)
            {
                PreslugItem newItem = new PreslugItem();
                newItem.StudentName = row["STUDENT_NAME"].ToString();
                newItem.StudentID = row["LOCAL_STUDENT_ID"].ToString();
                newItem.StateSchoolID = row["STATE_SCHOOL_ID"].ToString();
                newItem.Campus = row["SCHOOL2"].ToString();
                newItem.CourseTitle = row["DISTRICT_COURSE_TITLE"].ToString();
                newItem.CourseID = row["LOCAL_COURSE_ID"].ToString();
                newItem.Grade = row["GRADE_LEVEL"].ToString();
                newItem.TeacherName = row["TEACHER_NAME"].ToString();
                newItem.Period = row["PERIOD"].ToString();

                finalData.Add(newItem);
            }

            return finalData;
        }

        public static string ReturnRawCustomQuery(string testid)
        {
            string qs = Queries.GetCustomQuery.Replace("@testId", testid);

            DataSet ds = dbIFOracle.getDataRows(qs);
            string query = ds.Tables[0].Rows[0].ItemArray[0].ToString();

            return query;
        }


        public static List<string> ReturnStudentIDsWScansNotPreslugged(string testid, string campus)
        {
            string qs4 = Queries.GetStudentsWithScansNotInTestCriteria.Replace("@testId", testid);
            qs4 = qs4.Replace("@campus", campus);
            string customQuery = birIF.GetRawCustomQuery(testid);
            customQuery = customQuery.Replace("@school", "\'" + campus + "\'");
            qs4 = qs4.Replace("@query", customQuery);
            DataSet dsStudentsWithScansNotInTestCriteria = dbIFOracle.getDataRows(qs4);

            if (dsStudentsWithScansNotInTestCriteria.Tables.Count == 0)
                return null;

            List<string> finalData = new List<string>();

            foreach (DataRow row in dsStudentsWithScansNotInTestCriteria.Tables[0].Rows)
            {
                finalData.Add(row["STUDENT_ID"].ToString());
            }

            return finalData;
        }
    }
}