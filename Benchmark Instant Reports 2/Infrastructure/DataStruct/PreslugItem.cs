using Benchmark_Instant_Reports_2.Infrastructure.Entities;

namespace Benchmark_Instant_Reports_2.Infrastructure.DataStruct
{
    /// <summary>
    /// data item that is returned by a test's query (custom_query)
    /// </summary>
    public class PreslugItem
    {
        // select fields portion of a test custom_query:
        //  R.STUDENT_NAME, R.TEACHER_NAME || ' - ' || R.TEACHER_NBR,
        //  R.LOCAL_STUDENT_ID, R.STATE_SCHOOL_ID, R.SCHOOL2, R.DISTRICT_COURSE_TITLE,
        //  R.LOCAL_COURSE_ID, R.GRADE_LEVEL, R.TEACHER_NAME, R.PERIOD

        public string StudentName { get; set; }             
        public string StudentID { get; set; }               // key
        public string StateSchoolID { get; set; }
        public string Campus { get; set; }
        public string CourseTitle { get; set; }
        public string CourseID { get; set; }                // key
        public string Grade { get; set; }
        public string TeacherName { get; set; }
        public string Period { get; set; }
        public string TestID { get; set; }                  // key - added after the query

        /// <summary>
        /// default constructor - blank / empty / zero items
        /// </summary>
        public PreslugItem()
        {
            StudentName = "";
            StudentID = "";
            StateSchoolID = "";
            Campus = "";
            CourseTitle = "";
            CourseID = "";
            Grade = "";
            TeacherName = "";
            Period = "";
            TestID = "";
        }

        /// <summary>
        /// default constructor to create a blank item with just the key elements
        /// </summary>
        /// <param name="studentid">Student ID</param>
        /// <param name="courseid">Course ID</param>
        public PreslugItem(string studentid, string courseid, string testid)
        {
            StudentID = studentid;
            CourseID = courseid;
            TestID = testid;
        }


        /// <summary>
        /// constructor that takes a Roster entity item and converts it to a PreslugItem
        /// </summary>
        /// <param name="rosterItem">Roster item</param>
        public PreslugItem(Roster rosterItem, string testid)
        {
            StudentName = rosterItem.StudentName;
            StudentID = rosterItem.StudentID;
            StateSchoolID = rosterItem.SchoolID;
            Campus = rosterItem.CourseCampus;
            CourseTitle = rosterItem.CourseTitle;
            CourseID = rosterItem.CourseID;
            Grade = rosterItem.Grade;
            TeacherName = rosterItem.TeacherName;
            Period = rosterItem.Period;
            TestID = testid;
        }
    }



    /// <summary>
    /// the multi-part key to be used in a hashtable
    /// </summary>
    public class PreslugItemKey : ClassKey<PreslugItem>
    {
        /// <summary>
        /// create a hash key based on an instance of PreslugItem
        /// </summary>
        /// <param name="ClassReference">instance of a PreslugItem</param>
        public PreslugItemKey(PreslugItem ClassReference) : base(ClassReference) { }

        /// <summary>
        /// return the list of values in the key
        /// </summary>
        /// <returns>an array of objects that contain the values of the actual fields used in the key</returns>
        public override object[] GetKeyValues()
        {
            return new object[] 
            {
                ClassReference.StudentID,
                ClassReference.CourseID,
                ClassReference.TestID
            };
        }
    }

}