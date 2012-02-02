using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    public class StudentListItem
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string TeacherName { get; set; }
        public string Period { get; set; }
        public string CourseID { get; set; }
        public string Campus { get; set; }
        public string TestID { get; set; }


        public StudentListItem(string studentid, string studentname, string teachername,
            string period, string courseid, string campus, string testid)
        {
            StudentID = studentid;
            StudentName = studentname;
            TeacherName = teachername;
            Period = period;
            CourseID = courseid;
            Campus = campus;
            TestID = testid;
        }
    }


}