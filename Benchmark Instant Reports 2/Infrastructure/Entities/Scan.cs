using System;

namespace Benchmark_Instant_Reports_2.Infrastructure.Entities
{
    public class Scan
    {
        public DateTime DateScanned { get; set; }
        public int ScanSequence { get; set; }
        public string Imagepath { get; set; }
        public string StudentName { get; set; }
        public int StudentID { get; set; }
        public string TestID { get; set; }
        public string Language { get; set; }
        public string Exempt { get; set; }
        public string Preslugged { get; set; }
        public string AnswerString { get; set; }


        /// <summary>
        /// Default constructor, sets DateScanned to 1/1/1990
        /// </summary>
        public Scan()
        {
            DateScanned = DateTime.Parse("1/1/1990");
            ScanSequence = 0;
            Imagepath = "";
            StudentName = "";
            StudentID = 1;
            TestID = "";
            Language = "E";
            Exempt = "N";
            Preslugged = "N";
            AnswerString = "";
        }
    }


}