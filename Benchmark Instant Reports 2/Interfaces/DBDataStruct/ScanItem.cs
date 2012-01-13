using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Interfaces.DBDataStruct
{
    public class ScanItem
    {
        public string DateScannedStr { get; set; }
        public int ScanSequence { get; set; }
        public string Imagepath { get; set; }
        public string Name { get; set; }
        public string StudentID { get; set; }
        public string TestID { get; set; }
        public String Language { get; set; }
        public String Exempt { get; set; }
        public String PreSlugged { get; set; }
        public string Answers { get; set; }

        public ScanItem()
        {
            DateScannedStr = "";
            ScanSequence = 0;
            Imagepath = "";
            Name = "";
            StudentID = "";
            TestID = "";
            Language = "E";
            Exempt = "N";
            PreSlugged = "Y";
            Answers = "";
        }
    }
}