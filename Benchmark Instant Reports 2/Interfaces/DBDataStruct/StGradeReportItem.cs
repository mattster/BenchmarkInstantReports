using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Benchmark_Instant_Reports_2.Infrastructure;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    /// <summary>
    /// Student Grades report data item
    /// </summary>
    public class StGradeReportItem
    {
        public string StudentID { get; set; }           // key
        public string StudentName { get; set; }
        public string TestID { get; set; }              // key
        public DateTime ScanDate { get; set; }
        public string LetterGrade { get; set; }
        public int NumCorrect { get; set; }
        public int NumTotal { get; set; }
        public decimal PctCorrect { get; set; }
        public int PassNum { get; set; }
        public int CommendedNum { get; set; }
        public string Campus { get; set; }
        public string Teacher { get; set; }
        public string Period { get; set; }
        public string GradedAnswers { get; set; }
        public string GradedAnswersFormatted { get; set; }

        /// <summary>
        /// default constructor - blank / empty / zero values
        /// </summary>
        public StGradeReportItem()
        {
            StudentID = "";
            StudentName = "";
            TestID = "";
            ScanDate = DateTime.Now;
            LetterGrade = "Z";
            NumCorrect = 0;
            NumTotal = 0;
            PctCorrect = 0;
            PassNum = 0;
            CommendedNum = 0;
            Campus = "";
            Teacher = "";
            Period = "";
            GradedAnswers = "";
            GradedAnswersFormatted = "";
        }

        /// <summary>
        /// constructor to create a blank item with just the key elements
        /// </summary>
        /// <param name="studentid">Student ID</param>
        /// <param name="testid">Test ID</param>
        public StGradeReportItem(string studentid, string testid)
        {
            StudentID = studentid;
            TestID = testid;
        }
    }


    /// <summary>
    /// the multi-part key to be used in a hashtable
    /// </summary>
    public class StGradeReportItemKey : ClassKey<StGradeReportItem>
    {
        /// <summary>
        /// create a hash key based on an instance of StGradeReportItem
        /// </summary>
        /// <param name="ClassReference">instance of a StGradeReportItem</param>
        public StGradeReportItemKey(StGradeReportItem ClassReference) : base(ClassReference) { }

        /// <summary>
        /// return the list of values in the key
        /// </summary>
        /// <returns>an array of objects that contain the values of the actual fields used in the key</returns>
        public override object[] GetKeyValues()
        {
            return new object[] 
            {
                ClassReference.StudentID,
                ClassReference.TestID
            };
        }
    }

}