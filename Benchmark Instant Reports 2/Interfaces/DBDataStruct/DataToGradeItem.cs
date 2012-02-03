using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{

    public class DataToGradeItem
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string TeacherName { get; set; }
        public string Period { get; set; }
        public string CourseID { get; set; }
        public string Campus { get; set; }
        public string TestID { get; set; }
        public Scan ScanItem { get; set; }


        public DataToGradeItem()
        {
            StudentID = "";
            StudentName = "";
            TeacherName = "";
            Period = "";
            CourseID = "";
            Campus = "";
            TestID = "";
            ScanItem = null;
        }

        //public DataToGradeItem(string studentid, string studentname, string teachername,
        //    string period, string courseid, string campus, string testid, Scan scanitem)
        //{
        //    StudentID = studentid;
        //    StudentName = studentname;
        //    TeacherName = teachername;
        //    Period = period;
        //    CourseID = courseid;
        //    Campus = campus;
        //    TestID = testid;
        //    ScanItem = scanitem;
        //}
    }


}