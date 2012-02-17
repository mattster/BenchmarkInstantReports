
namespace Benchmark_Instant_Reports_2.Infrastructure.Entities
{
    public class Roster
    {
        public string StudentName { get; set; }
        public string StudentID { get; set; }
        public string LEPCode { get; set; }
        public string SPEDFlag { get; set; }
        public string Grade { get; set; }
        public string SchoolID { get; set; }
        public string TeacherNum { get; set; }
        public string TeacherName { get; set; }
        public string Active { get; set; }
        public string CourseID { get; set; }
        public string CourseTitle { get; set; }
        public string Semester { get; set; }
        public string Period { get; set; }
        public string HomeCampus { get; set; }
        public string USYears { get; set; }
        public string FirstYear { get; set; }
        public string ModifiedFlag { get; set; }
        public string CourseCampus { get; set; }


        public Roster()
        {
            StudentName = "";
            StudentID = "";
            LEPCode = "";
            SPEDFlag = "";
            Grade = "";
            SchoolID = "";
            TeacherNum = "";
            TeacherName = "";
            Active = "";
            CourseID = "";
            CourseTitle = "";
            Semester = "";
            Period = "";
            HomeCampus = "";
            USYears = "";
            FirstYear = "";
            ModifiedFlag = "";
            CourseCampus = "";
        }
    }
}