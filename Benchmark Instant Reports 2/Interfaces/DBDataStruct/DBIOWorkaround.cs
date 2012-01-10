using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Benchmark_Instant_Reports_2.References;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    public class DBIOWorkaround
    {
        public static List<StudentScanDataItem> ReturnStudentScanDataItemsFromQ(string qs)
        {
            DataSet ds = birIF.makeUniqueDataSet(dbIFOracle.getDataRows(qs));
            List<StudentScanDataItem> returnList = new List<StudentScanDataItem>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                StudentScanDataItem newitem = new StudentScanDataItem();
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
            item.ScanSequence = (int)row["SCANNED_SEQUENCE"];
            item.Imagepath = row["IMAGEPATH"].ToString();
            item.Name = row["NAME"].ToString();
            item.StudentID = row["STUDENT_ID"].ToString();
            item.TestID = row["TEST_ID"].ToString();
            item.Language = (char)row["LANGUAGE"];
            item.Exempt = (char)row["EXEMPT"];
            item.PreSlugged = (char)row["PRESLUGGED"];
            item.Answers = row["ANSWERS"].ToString();

            return item;
        }
    }
}