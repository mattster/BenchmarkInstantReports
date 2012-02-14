using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Infrastructure.DataStruct
{
    /// <summary>
    /// data structure for data that is ready to grade
    /// </summary>
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
    }


}