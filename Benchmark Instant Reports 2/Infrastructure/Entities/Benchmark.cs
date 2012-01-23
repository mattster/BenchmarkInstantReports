using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Infrastructure.Entities
{
    public class Benchmark
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
    }
}